using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace WeatherForecast
{
	public class WeatherEntry : MonoBehaviour
	{
		[SerializeField] private Image icon;
		[SerializeField] private TMP_Text day;
		[SerializeField] private TMP_Text temperatureHigh;
		[SerializeField] private TMP_Text temperatureLow;

		public void SetDayText(string s)
		{
			day.text = s;
		}

		public void SetIconSprite(Sprite s)
		{
			icon.sprite = s;
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