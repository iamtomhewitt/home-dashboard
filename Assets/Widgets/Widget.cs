using UnityEngine;
using UnityEngine.UI;
using System;
using SimpleJSON;
using TMPro;

/// <summary>
/// The base of all widgets, sharing all common variables.
/// </summary>
public abstract class Widget : MonoBehaviour
{
	[Header("Widget Settings")]
	[SerializeField] private TMP_Text titleText;
	[SerializeField] private TMP_Text lastUpdatedText;
	[SerializeField] private Image widgetBackground;
	[SerializeField] private string widgetConfigKey;

	private Color widgetColour;
	private Color textColour;
	private Color titleColour;
	private string title;
	private string timeUnit;
	private float repeatRate;

	public abstract void Run();
	public abstract void ReloadConfig();

	public virtual void Start()
	{
		this.ReloadConfig();
		this.Initialise();
		InvokeRepeating("Run", 0f, GetRepeatRateInSeconds());
	}

	public void Initialise()
	{
		JSONNode config = Config.instance.GetWidgetConfig()[widgetConfigKey];
		
		widgetColour= Colours.ToColour(config["colour"]);
		textColour 	= Colours.ToColour(config["textColour"]);
		titleColour	= Colours.ToColour(config["titleColour"]);
		title 		= config["title"];
		repeatRate 	= config["repeatRate"];
		timeUnit 	= config["repeatTime"];

		UpdateLastUpdatedText();
		SetTitleText(title);
		SetWidgetColour(widgetColour);
		SetLastUpdatedTextColour(textColour);
		SetTitleTextColour(titleColour);
	}

	private float GetRepeatRateInSeconds()
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

	public Color GetTitleColour()
	{
		return titleColour;
	}

	public string GetWidgetTitle()
	{
		return title;
	}

	public string GetWidgetConfigKey()
	{
		return widgetConfigKey;
	}
}