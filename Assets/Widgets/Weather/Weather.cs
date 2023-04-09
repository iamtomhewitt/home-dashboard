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

			for (int i = 0; i < 3; i++)
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
				string temperature = string.Format("{0}°", Mathf.RoundToInt((float)data["temperature"]));

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
			JSONNode daily = json["forecast"]["forecastday"];

			for (int i = 0; i < daily.Count; i++)
			{
				JSONNode day = daily[i];
				JSONObject weatherForDay = new JSONObject();
				weatherForDay["temperature"] = day["day"]["avgtemp_c"];
				weatherForDay["icon"] = day["day"]["condition"]["code"];
				weatherForDay["time"] = day["date_epoch"];
				data.Add(weatherForDay);
			}

			return data;
		}

		private List<JSONNode> GetHourlyWeatherData(JSONNode json)
		{
			List<JSONNode> data = new List<JSONNode>();

			JSONNode today = json["forecast"]["forecastday"][0];
			JSONNode hourly = today["hour"];

			JSONObject weatherForNow = new JSONObject();
			weatherForNow["temperature"] = json["current"]["temp_c"];
			weatherForNow["icon"] = json["current"]["condition"]["code"];
			weatherForNow["time"] = json["location"]["localtime_epoch"];

			data.Add(weatherForNow);

			for (int i = 0; i < hourly.Count; i++)
			{
				JSONNode hour = hourly[i];
				DateTime date = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(hour["time_epoch"]);
				DateTime now = DateTime.Now;

				if (now < date) {
					JSONObject weatherForHour = new JSONObject();
					weatherForHour["temperature"] = hour["temp_c"];
					weatherForHour["icon"] = hour["condition"]["code"];
					weatherForHour["time"] = hour["time_epoch"];
					data.Add(weatherForHour);
				}
			}

			return data;
		}

		/// <summary>
		///	Get a sprite that matches the weather string. Also realign as some characters have extra top space for some reason.
		/// Codes can be found at https://www.weatherapi.com/docs/weather_conditions.json
		/// </summary>
		private string GetFontCodeFor(string code)
		{
			switch (code)
			{
				// Sunny / clear
				case "1000":
					return "1";

				// Partly cloudy
				case "1003":
					return "A";

				// Rain
				case "1189":
				case "1063":
					return "K";

				// Sleet
				case "1207":
					return "W";

				// Snow
				case "1219":
					return "I";

				// Cloudy
				case "1006":
				case "1009":
					return "3";

				// Fog
				case "1135":
					return "…";
			}

			WidgetLogger.instance.Log(this, "Could not find: " + code);
			return "“";
		}
	}
}