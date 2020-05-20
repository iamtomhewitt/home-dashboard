using UnityEngine;
using System.Collections.Generic;
using TMPro;

namespace WeatherForecast
{
	public class WeatherEntry : MonoBehaviour
	{
		[SerializeField] private TMP_Text icon;
		[SerializeField] private TMP_Text day;
		[SerializeField] private TMP_Text temperature;

		private List<string> outOfLineCharacters = new List<string> { "1", "c", "K", "W", "I", "," };

		public void SetDayText(string s)
		{
			day.text = s;
		}

		public void SetIcon(string s)
		{
			icon.text = s;

			// Fonts can get out of line
			icon.transform.localPosition = GetPositionFor(s);
		}

		public void SetTemperatureText(string s)
		{
			temperature.text = s;
		}

		public void SetIconColour(Color colour)
		{
			icon.color = colour;
		}

		public void SetDayColour(Color colour)
		{
			day.color = colour;
		}

		public void SetTemperatureTextColour(Color colour)
		{
			temperature.color = colour;
		}

		private Vector2 GetPositionFor(string iconCode)
		{
			switch (iconCode)
			{
				case "1":
					return new Vector2(0.19f, -0.57f);
				case "3":
					return new Vector2(0.44f, 0f);
				case "6":
					return new Vector2(0.2f, -0.4f);
				case "A":
					return new Vector2(0.63801f, -0.00015625f);
				case "c":
					return new Vector2(-0.16f, 0.36f);
				case "K":
					return new Vector2(0.2f, 0.52f);
				case "W":
					return new Vector2(0.2f, 0.52f);
				case "I":
					return new Vector2(0.2f, 0.52f);
				case ",":
					return new Vector2(0.36f, 0f);
				case "…":
					return new Vector2(0.2f, 0f);
				default:
					return Vector2.zero;
			}
		}
	}
}