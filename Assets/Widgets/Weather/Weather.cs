using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Collections.Generic;
using Dialog;
using Requests;
using SimpleJSON;
using TMPro;

namespace WeatherForecast
{
	public class Weather : Widget
	{
		[Header("Weather Settings")]
		[SerializeField] private WeatherEntry[] hourlyWeatherEntries;
		[SerializeField] private WeatherEntry[] dailyWeatherEntries;

		private Color spriteColour;
		private List<string> outOfLineCharacters = new List<string> { "K", "W", "I" };

		private string apiKey;
		private string latitude;
		private string longitude;
		private int dayOffset = 2;

		public override void ReloadConfig()
		{
			JSONNode config = Config.instance.GetWidgetConfig()[this.GetWidgetConfigKey()];
			apiKey = config["apiKey"];
			latitude = config["latitude"];
			longitude = config["longitude"];
			spriteColour = Colours.ToColour(config["spriteColour"]);
		}

		public override void Run()
		{
			this.ReloadConfig();
			StartCoroutine(RunRoutine());
			this.UpdateLastUpdatedText();
		}

		private IEnumerator RunRoutine()
		{
			UnityWebRequest request = Postman.CreateGetRequest(Endpoints.instance.WEATHER(apiKey, latitude, longitude));
			yield return request.SendWebRequest();

			JSONNode json = JSON.Parse(request.downloadHandler.text);

			bool ok = request.error == null ? true : false;
			if (!ok)
			{
				WidgetLogger.instance.Log(this, "Error: " + request.error);
				yield break;
			}

			JSONNode weeklyWeather = json["daily"]["data"];

			List<JSONNode> hourlyWeather = new List<JSONNode>();
			hourlyWeather.Add(json["hourly"]["data"][1]);
			hourlyWeather.Add(json["hourly"]["data"][3]);
			hourlyWeather.Add(json["hourly"]["data"][6]);

			foreach (JSONNode n in hourlyWeather)
			{
				string time = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(n["time"]).ToLocalTime().TimeOfDay.ToString();
				string iconCode = GetFontCodeFor(n["icon"]);
				string temperature = string.Format("{0}°", n["temperature"]);

				print(string.Format("{0} {1} {2}", time, iconCode, temperature));
			}

			for (int i = 0; i < dailyWeatherEntries.Length; i++)
			{
				JSONNode day = weeklyWeather[i + dayOffset];
				WeatherEntry entry = dailyWeatherEntries[i];

				DateTime date = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(day["time"]);

				entry.SetDayText(date.DayOfWeek.ToString());
				entry.SetDayColour(GetTextColour());

				entry.SetIcon(GetFontCodeFor(day["icon"]));
				entry.SetIconColour(spriteColour);

				entry.SetTemperatureText(Mathf.RoundToInt((float)day["temperatureHigh"]).ToString() + "°");
				entry.SetTemperatureTextColour(GetTextColour());
			}
		}

		/// <summary>
		///	Get a sprite that matches the weather string. Also realign as some characters have extra top space for some reason.
		/// </summary>
		private string GetFontCodeFor(string weatherName)
		{
			switch (weatherName)
			{
				case "clear-day":
					return "1";

				case "partly-cloudy-day":
					return "A";

				case "partly-cloudy-night":
					return "c";

				case "rain":
					return "K";

				case "sleet":
					return "W";

				case "snow":
					return "I";

				case "wind":
					return ",";

				case "cloudy":
					return "3";

				case "clear-night":
					return "6";

				case "fog":
					return "…";
			}

			WidgetLogger.instance.Log(this, "Could not find: " + weatherName);
			return "“";
		}
	}
}
