using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System;
using System.Collections;
using SimpleJSON;
using Dialog;

namespace WeatherForecast
{
	public class Weather : Widget
	{
		[Space(15f)]

		[SerializeField] private Text currentSummary;
		[SerializeField] private Image currentIcon;
		[SerializeField] private Text currentTemperature;

		[SerializeField] private WeatherEntry[] weatherEntries;
		[SerializeField] private Sprite[] weatherSprites;

		private string apiKey;
		private string latitude;
		private string longitude;

		private void Start()
		{
			apiKey 		= Config.instance.GetConfig()["apiKeys"]["weather"];
			latitude 	= Config.instance.GetConfig()["weather"]["latitude"];
			longitude 	= Config.instance.GetConfig()["weather"]["longitude"];

			this.Initialise();
			InvokeRepeating("Run", 0f, RepeatRateInSeconds());
		}

		public override void Run()
		{
			StartCoroutine(RunRoutine());
			this.UpdateLastUpdatedText();
		}

		private IEnumerator RunRoutine()
		{
			string url = "https://api.darksky.net/forecast/" + apiKey + "/" + latitude + "," + longitude + "?units=uk";

			UnityWebRequest request = UnityWebRequest.Get(url);
			yield return request.SendWebRequest();
			string response = request.downloadHandler.text;

			JSONNode json = JSON.Parse(response);

			bool ok = request.error == null ? true : false;
			if (!ok)
			{
				WidgetLogger.instance.Log(this, "Error: " + request.error);
			}

			currentSummary.text = json["currently"]["summary"];
			currentIcon.sprite = GetSpriteForName(json["currently"]["icon"]);
			currentTemperature.text = Mathf.RoundToInt((float)json["currently"]["temperature"]).ToString() + "°";

			for (int i = 0; i < weatherEntries.Length; i++)
			{
				JSONNode day = json["daily"]["data"][i + 1];
				WeatherEntry entry = weatherEntries[i];

				DateTime date = new DateTime(1970, 1, 1, 0, 0, 0, 0);
				date = date.AddSeconds(day["time"]);

				entry.SetDayText(date.DayOfWeek.ToString());
				entry.SetIconSprite(GetSpriteForName(day["icon"]));
				entry.SetTemperatureHighText(Mathf.RoundToInt((float)day["temperatureHigh"]).ToString() + "°");
				entry.SetTemperatureLowText(Mathf.RoundToInt((float)day["temperatureLow"]).ToString() + "°");
			}
		}

		private Sprite GetSpriteForName(string weatherName)
		{
			foreach (Sprite weatherSprite in weatherSprites)
			{
				if (weatherSprite.name == weatherName)
				{
					return weatherSprite;
				}
			}

			print("Could not find: " + weatherName);
			return null;
		}
	}
}
