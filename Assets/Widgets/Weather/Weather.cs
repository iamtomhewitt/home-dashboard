using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System;
using System.Collections;
using SimpleJSON;
using Dialog;
using Requests;

namespace WeatherForecast
{
	public class Weather : Widget
	{
		[Header("Weather Settings")]
		[SerializeField] private Text currentSummary;
		[SerializeField] private Image currentIcon;
		[SerializeField] private Text currentTemperature;

		[SerializeField] private WeatherEntry[] weatherEntries;
		[SerializeField] private Sprite[] weatherSprites;

		private Color spriteColour;

		private string apiKey;
		private string latitude;
		private string longitude;

		public override void ReloadConfig()
		{
			JSONNode config = Config.instance.GetWidgetConfig()[this.GetWidgetConfigKey()];
			apiKey 		= config["apiKey"];
			latitude 	= config["latitude"];
			longitude 	= config["longitude"];
			spriteColour= Colours.ToColour(config["spriteColour"]);
		}

		public override void Run()
		{
			this.ReloadConfig();
			StartCoroutine(RunRoutine());
			this.UpdateLastUpdatedText();
		}

		private IEnumerator RunRoutine()
		{
			UnityWebRequest request = UnityWebRequest.Get(Endpoints.WEATHER(apiKey, latitude, longitude));
			yield return request.SendWebRequest();

			JSONNode json = JSON.Parse(request.downloadHandler.text);

			bool ok = request.error == null ? true : false;
			if (!ok)
			{
				WidgetLogger.instance.Log(this, "Error: " + request.error);
				yield break;
			}

			currentSummary.text = json["current"]["summary"];
			currentSummary.color = GetTitleColour();

			currentIcon.sprite = GetSpriteForName(json["current"]["icon"]);
			currentIcon.color = spriteColour;

			currentTemperature.text = Mathf.RoundToInt((float)json["current"]["temperature"]).ToString() + "°";
			currentTemperature.color = GetTitleColour();

			for (int i = 0; i < weatherEntries.Length; i++)
			{
				JSONNode day = json["week"][i];
				WeatherEntry entry = weatherEntries[i];

				entry.SetDayText(day["day"]);
				entry.SetDayColour(GetTextColour());
				
				entry.SetIconSprite(GetSpriteForName(day["icon"]));
				entry.SetIconColour(spriteColour);
				
				entry.SetTempHighText(Mathf.RoundToInt((float)day["temperatureHigh"]).ToString() + "°");
				entry.SetTempHighColour(GetTextColour());
				
				entry.SetTempLowText(Mathf.RoundToInt((float)day["temperatureLow"]).ToString() + "°");
				entry.SetTempLowColour(GetTextColour());
			}
		}

		/// <summary>
		///	Get a sprite that matches the weather string.
		/// </summary>
		private Sprite GetSpriteForName(string weatherName)
		{
			foreach (Sprite weatherSprite in weatherSprites)
			{
				if (weatherSprite.name == weatherName)
				{
					return weatherSprite;
				}
			}

			WidgetLogger.instance.Log(this, "Could not find: " + weatherName);
			return null;
		}
	}
}
