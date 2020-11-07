using Dialog;
using Requests;
using SimpleJSON;
using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine.Networking;
using UnityEngine;

namespace WeatherForecast
{
	public class Weather : Widget
	{
		[Header("Weather Settings")]
		[SerializeField] private WeatherEntry[] dailyWeatherEntries;
		[SerializeField] private WeatherEntry[] hourlyWeatherEntries;

		private Color spriteColour;

		public override void ReloadConfig()
		{
			JSONNode config = this.GetConfig();
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
			UnityWebRequest request = Postman.CreateGetRequest(Endpoints.instance.WEATHER());
			yield return request.SendWebRequest();

			bool ok = request.error == null ? true : false;
			if (!ok)
			{
				WidgetLogger.instance.Log(this, "Error: " + request.error);
				yield break;
			}

			JSONNode json = JSON.Parse(request.downloadHandler.text);
			List<JSONNode> hourlyWeatherData = GetHourlyWeatherData(json);
			List<JSONNode> dailyWeatherData = GetDailyWeatherData(json);

			for (int i = 0; i < hourlyWeatherData.Count; i++)
			{
				JSONNode data = hourlyWeatherData[i];
				TimeSpan time = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(data["time"]).ToLocalTime().TimeOfDay;
				string iconCode = GetFontCodeFor(data["icon"]);
				string temperature = string.Format("{0}°", Mathf.RoundToInt((float)data["temperature"]));

				WeatherEntry entry = hourlyWeatherEntries[i];
				entry.SetDayColour(GetTextColour());
				entry.SetDayText(string.Format("{0:D2}:{1:D2}", time.Hours, time.Minutes));
				entry.SetIcon(iconCode);
				entry.SetIconColour(spriteColour);
				entry.SetTemperatureText(temperature);
				entry.SetTemperatureTextColour(GetTextColour());
			}

			for (int i = 0; i < dailyWeatherData.Count; i++)
			{
				JSONNode data = dailyWeatherData[i];
				DateTime date = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(data["time"]);
				string day = date.DayOfWeek.ToString().Substring(0, 3);
				string iconCode = GetFontCodeFor(data["icon"]);
				string temperature = string.Format("{0}°", Mathf.RoundToInt((float)data["temperatureHigh"]));

				WeatherEntry entry = dailyWeatherEntries[i];
				entry.SetDayColour(GetTextColour());
				entry.SetDayText(day.ToLower());
				entry.SetIcon(iconCode);
				entry.SetIconColour(spriteColour);
				entry.SetTemperatureText(temperature);
				entry.SetTemperatureTextColour(GetTextColour());
			}
		}

		private List<JSONNode> GetDailyWeatherData(JSONNode json)
		{
			List<JSONNode> data = new List<JSONNode>();
			for (int i = 0; i < dailyWeatherEntries.Length; i++)
			{
				data.Add(json["daily"]["data"][i + 1]);
			}
			return data;
		}

		private List<JSONNode> GetHourlyWeatherData(JSONNode json)
		{
			List<JSONNode> data = new List<JSONNode>();
			data.Add(json["hourly"]["data"][1]);
			data.Add(json["hourly"]["data"][3]);
			data.Add(json["hourly"]["data"][6]);
			return data;
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