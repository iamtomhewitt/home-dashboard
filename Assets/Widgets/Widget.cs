using UnityEngine;
using UnityEngine.UI;
using System;
using SimpleJSON;

/// <summary>
/// The base of all widgets, sharing all common variables.
/// </summary>
public abstract class Widget : MonoBehaviour
{
	[Header("Widget Settings")]
	[SerializeField] private Text titleText;
	[SerializeField] private Text lastUpdatedText;
	[SerializeField] private Image widgetBackground;
	[SerializeField] private string widgetConfigKey;

	private Color widgetColour;
	private Color textColour;
	private string title;
	private float repeatRate;
	private string timeUnit;

	public abstract void Run();
	public abstract void ReloadConfig();

	public void Initialise()
	{
		JSONNode config = Config.instance.GetConfig()[widgetConfigKey];
		
		widgetColour= ToColour(config["colour"]);
		textColour 	= ToColour(config["textColour"]);
		title 		= config["title"];
		repeatRate 	= config["repeatRate"];
		timeUnit 	= config["repeatTime"];

		UpdateLastUpdatedText();
		SetTitleText(title);
		SetWidgetColour(widgetColour);
		SetLastUpdatedTextColour(textColour);
		SetTitleTextColour(textColour);
	}

	public float RepeatRateInSeconds()
	{
		float seconds = 0f;

		switch (timeUnit)
		{
			case "seconds":
				seconds = repeatRate;
				break;

			case "minutes":
				seconds = repeatRate * 60f;
				break;

			case "hours":
				seconds = repeatRate * 3600f;
				break;

			case "days":
				seconds = repeatRate * 86400f;
				break;

			default:
				Debug.Log("Unknown unit: " + timeUnit);
				seconds = 600f;
				break;
		}

		return seconds;
	}

	public void UpdateLastUpdatedText()
	{
		lastUpdatedText.text = "Last Updated: " + DateTime.Now.ToString("HH:mm");
		lastUpdatedText.color = textColour;
	}

	private void SetTitleText(string s)
	{
		titleText.text = s;
	}

	public void SetWidgetColour(Color colour)
	{
		widgetBackground.color = colour;
	}

	public Color GetWidgetColour()
	{
		return widgetColour;
	}
	
	private void SetLastUpdatedTextColour(Color colour)
	{
		lastUpdatedText.color = colour;
	}
	
	public void SetTitleTextColour(Color colour)
	{
		titleText.color = colour;
	}

	public Color GetTextColour()
	{
		return textColour;
	}

	public string GetWidgetTitle()
	{
		return title;
	}

	public string GetWidgetConfigKey()
	{
		return widgetConfigKey;
	}

	public Color ToColour(string hex)
	{
		Color colour;

		if (ColorUtility.TryParseHtmlString(hex, out colour))
		{
            return colour;
		}
		else
		{
			return Color.white;
		}
	}
}
