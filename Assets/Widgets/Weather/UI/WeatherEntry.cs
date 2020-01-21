using UnityEngine;
using UnityEngine.UI;

namespace WeatherForecast
{
	public class WeatherEntry : MonoBehaviour
	{
		[SerializeField] private Text day;
		[SerializeField] private Image icon;
		[SerializeField] private Text temperatureHigh;
		[SerializeField] private Text temperatureLow;

		public void SetDayText(string s)
		{
			day.text = s;
		}

		public void SetIconSprite(Sprite s)
		{
			icon.sprite = s;
		}

		public void SetTemperatureHighText(string s)
		{
			temperatureHigh.text = s;
		}

		public void SetTemperatureLowText(string s)
		{
			temperatureLow.text = s;
		}

		public void SetColour(Color colour)
		{
			icon.color = colour;
			day.color = colour;
			temperatureHigh.color = colour;
			temperatureLow.color = colour;			
		}
	}
}
