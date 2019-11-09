using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System;
using System.Collections;
using JsonResponse;

namespace WeatherForecast
{
	public class Weather : Widget
	{
		[Space(15f)]
		[SerializeField] private string latitude;
		[SerializeField] private string longitude;

		[SerializeField] private Text currentSummary;
		[SerializeField] private Image currentIcon;
		[SerializeField] private Text currentTemperature;

		[SerializeField] private WeatherEntry[] weatherEntries;
		[SerializeField] private Sprite[] weatherSprites;
		[SerializeField] private Config config;

		private string apiKey;

		private void Start()
		{
			apiKey = config.GetConfig()["apiKeys"]["weather"];

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
			string jsonResponse = request.downloadHandler.text;

			WeatherJsonResponse response = JsonUtility.FromJson<WeatherJsonResponse>(jsonResponse);

			currentSummary.text = response.currently.summary;
			currentIcon.sprite = GetSpriteForName(response.currently.icon);
			currentTemperature.text = Mathf.RoundToInt((float)response.currently.temperature).ToString() + "°";

			for (int i = 0; i < weatherEntries.Length; i++)
			{
				Data day = response.daily.data[i + 2];
				WeatherEntry entry = weatherEntries[i];

				DateTime date = new DateTime(1970, 1, 1, 0, 0, 0, 0);
				date = date.AddSeconds(day.time);

				entry.SetDayText(date.DayOfWeek.ToString());
				entry.SetIconSprite(GetSpriteForName(day.icon));
				entry.SetTemperatureHighText(Mathf.RoundToInt((float)day.temperatureHigh).ToString() + "°");
				entry.SetTemperatureLowText(Mathf.RoundToInt((float)day.temperatureLow).ToString() + "°");
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
