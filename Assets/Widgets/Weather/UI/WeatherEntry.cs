using UnityEngine;
using System.Collections.Generic;
using TMPro;

namespace WeatherForecast
{
    public class WeatherEntry : MonoBehaviour
    {
        [SerializeField] private TMP_Text icon;
        [SerializeField] private TMP_Text day;
        [SerializeField] private TMP_Text temperatureHigh;
        [SerializeField] private TMP_Text temperatureLow;

		private List<string> outOfLineCharacters = new List<string> {"1", "c", "K", "W", "I", ","};

        public void SetDayText(string s)
        {
            day.text = s;
        }

        public void SetIcon(string s)
        {
            icon.text = s;

            // Fonts can get out of line
            float y = outOfLineCharacters.Contains(icon.text) ? 0f : 0.84f;
			icon.transform.localPosition = new Vector3(icon.transform.localPosition.x, y, icon.transform.localPosition.z);
        }

        public void SetTempHighText(string s)
        {
            temperatureHigh.text = s;
        }

        public void SetTempLowText(string s)
        {
            temperatureLow.text = s;
        }

        public void SetIconColour(Color colour)
        {
            icon.color = colour;
        }

        public void SetDayColour(Color colour)
        {
            day.color = colour;
        }

        public void SetTempHighColour(Color colour)
        {
            temperatureHigh.color = colour;
        }

        public void SetTempLowColour(Color colour)
        {
            temperatureLow.color = colour;
        }
    }
}