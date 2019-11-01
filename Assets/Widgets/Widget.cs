using UnityEngine;
using UnityEngine.UI;
using System;

public abstract class Widget : MonoBehaviour
{
	[SerializeField] private Text titleText;

	[Space()]
	[SerializeField] private Text lastUpdatedText;
	[SerializeField] private Image widgetBackground;

	[Space()]
	[SerializeField] private Color widgetColour;
	[SerializeField] private Color textColour;

	[Space()]
	[SerializeField] private string title;

	[Space()]
	[SerializeField] private float repeatRate;
	[SerializeField] private TimeUnit timeUnit;

	public abstract void Run();

	public void Initialise()
	{
		UpdateLastUpdatedText();
		SetTitleText(title);
		SetColour(widgetColour);
		SetTextColour(textColour);
	}

	public float RepeatRateInSeconds()
	{
		float seconds = 0f;

		switch (timeUnit)
		{
			case TimeUnit.Seconds:
				seconds = repeatRate;
				break;

			case TimeUnit.Minutes:
				seconds = repeatRate * 60f;
				break;

			case TimeUnit.Hours:
				seconds = repeatRate * 3600f;
				break;

			case TimeUnit.Days:
				seconds = repeatRate * 86400f;
				break;

			default:
				print("Unknown unit: " + timeUnit);
				seconds = 600f;
				break;
		}

		// print(amount + " " + unit + ": " + seconds + " seconds");

		return seconds;
	}

	public void UpdateLastUpdatedText()
	{
		lastUpdatedText.text = "Last Updated: " + DateTime.Now.ToString("HH:mm");
	}

	private void SetTitleText(string s)
	{
		titleText.text = s;
	}

	public void SetColour(Color colour)
	{
		widgetBackground.color = colour;
	}

	private void SetTextColour(Color colour)
	{
		titleText.color = colour;
		lastUpdatedText.color = colour;
	}

	[System.Serializable]
	public enum TimeUnit
	{
		Seconds,
		Minutes,
		Hours,
		Days
	}
}
