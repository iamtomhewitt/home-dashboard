using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using Dialog;
using Requests;
using TMPro;

namespace WeatherForecast
{
    public class Weather : Widget
    {
        [Header("Weather Settings")]
        [SerializeField] private TMP_Text currentSummary;
        [SerializeField] private TMP_Text currentTemperature;
        [SerializeField] private TMP_Text currentIcon;
        [SerializeField] private WeatherEntry[] weatherEntries;

        private Color spriteColour;
		private List<string> outOfLineCharacters = new List<string> {"K", "W", "I"};

        private string apiKey;
        private string latitude;
        private string longitude;
		
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
            UnityWebRequest request = Postman.CreateGetRequest(Endpoints.WEATHER(apiKey, latitude, longitude));
            yield return request.SendWebRequest();

            JSONNode json = JSON.Parse(request.downloadHandler.text);

            bool ok = request.error == null ? true : false;
            if (!ok)
            {
                WidgetLogger.instance.Log(this, "Error: " + request.error);
                yield break;
            }

            JSONNode currentWeather = json["currently"];
            JSONNode weeklyWeather = json["daily"]["data"];

            currentSummary.text = currentWeather["summary"];
            currentSummary.color = GetTitleColour();

            currentIcon.text = GetFontCodeFor(currentWeather["icon"]);
            currentIcon.color = spriteColour;

			// Fonts can get out of line
            float y = outOfLineCharacters.Contains(currentIcon.text) ? 4.5f : 0f;
			currentIcon.transform.localPosition = new Vector3(currentIcon.transform.localPosition.x, y, currentIcon.transform.localPosition.z);

            currentTemperature.text = Mathf.RoundToInt((float)currentWeather["temperature"]).ToString() + "°";
            currentTemperature.color = GetTitleColour();

            for (int i = 0; i < weatherEntries.Length; i++)
            {
                JSONNode day = weeklyWeather[i + 1];
                WeatherEntry entry = weatherEntries[i];

                DateTime date = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(day["time"]);

                entry.SetDayText(date.DayOfWeek.ToString());
                entry.SetDayColour(GetTextColour());

                entry.SetIcon(GetFontCodeFor(day["icon"]));
                entry.SetIconColour(spriteColour);

                entry.SetTempHighText(Mathf.RoundToInt((float)day["temperatureHigh"]).ToString() + "°");
                entry.SetTempHighColour(GetTextColour());

                entry.SetTempLowText(Mathf.RoundToInt((float)day["temperatureLow"]).ToString() + "°");
                entry.SetTempLowColour(GetTextColour());
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
