﻿using UnityEngine;
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

		private string apiKey;
		private string latitude;
		private string longitude;
		private string spriteColour;

		public override void ReloadConfig()
		{
			JSONNode config = Config.instance.GetWidgetConfig()[this.GetWidgetConfigKey()];
			apiKey 		= config["apiKey"];
			latitude 	= config["latitude"];
			longitude 	= config["longitude"];
			spriteColour= config["spriteColour"];
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
			}

			currentSummary.text = json["currently"]["summary"];
			currentSummary.color = GetTextColour();

			currentIcon.sprite = GetSpriteForName(json["currently"]["icon"]);
			currentIcon.color = Colours.ToColour(spriteColour);

			currentTemperature.text = Mathf.RoundToInt((float)json["currently"]["temperature"]).ToString() + "°";
			currentTemperature.color = GetTextColour();

			for (int i = 0; i < weatherEntries.Length; i++)
			{
				JSONNode day = json["daily"]["data"][i + 1];
				WeatherEntry entry = weatherEntries[i];

				DateTime date = new DateTime(1970, 1, 1, 0, 0, 0, 0);
				date = date.AddSeconds(day["time"]);

				entry.SetDayText(date.DayOfWeek.ToString());
				entry.SetIconSprite(GetSpriteForName(day["icon"]));
				entry.SetColour(Colours.ToColour(spriteColour));
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

			WidgetLogger.instance.Log(this, "Could not find: " + weatherName);
			return null;
		}
	}
}
