using UnityEngine;
using UnityEngine.UI;
using System;

public abstract class Widget : MonoBehaviour
{
	[SerializeField] private Text titleText;
	[SerializeField] private Text lastUpdatedText;
	[SerializeField] private Image widgetBackground;

	[SerializeField] private Color widgetColour;
	[SerializeField] private Color textColour;

	[SerializeField] private string title;

	[Space()]
	public float repeatRate;
	public TimeUnit timeUnit;

	public abstract void Run();

	public void Initialise()
	{
		UpdateLastUpdatedText();
		SetTitleText();
		SetColour(widgetColour);
		SetTextColour();
	}

	public float ToSeconds(TimeUnit unit, float amount)
	{
		float seconds = 0f;

		switch (unit)
		{
			case TimeUnit.Seconds:
				seconds = amount;
				break;

			case TimeUnit.Minutes:
				seconds = amount * 60f;
				break;

			case TimeUnit.Hours:
				seconds = amount * 3600f;
				break;

			case TimeUnit.Days:
				seconds = amount * 86400f;
				break;

			default:
				print("Unknown unit: " + unit);
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

	private void SetTitleText()
	{
		titleText.text = title;
	}

	public void SetColour(Color colour)
	{
		widgetBackground.color = colour;
	}

	private void SetTextColour()
	{
		titleText.color = textColour;
		lastUpdatedText.color = textColour;
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
