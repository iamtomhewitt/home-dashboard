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
	private string sleepStart;
	private string sleepEnd;
	private float repeatRate;
	private bool sleeping;

	public abstract void Run();
	public abstract void ReloadConfig();

	public virtual void Start()
	{
		this.ReloadConfig();
		this.Initialise();
		this.Run();
		InvokeRepeating("RunIfNotSleeping", 0f, GetRepeatRateInSeconds());
	}

	public void Initialise()
	{
		JSONNode config = Config.instance.GetWidgetConfig()[widgetConfigKey];

		widgetColour = Colours.ToColour(config["colour"]);
		textColour = Colours.ToColour(config["textColour"]);
		titleColour = Colours.ToColour(config["titleColour"]);
		title = config["title"];
		repeatRate = config["repeatRate"];
		timeUnit = config["repeatTime"];
		sleepStart = config["sleepStart"];
		sleepEnd = config["sleepEnd"];

		UpdateLastUpdatedText();
		SetTitleText(title);
		SetWidgetColour(widgetColour);
		SetLastUpdatedTextColour(textColour);
		SetTitleTextColour(titleColour);
	}

	private void RunIfNotSleeping()
	{
		TimeSpan start = TimeSpan.Parse(sleepStart);
		TimeSpan end = TimeSpan.Parse(sleepEnd);
		TimeSpan now = DateTime.Now.TimeOfDay;

		if (start <= end)
		{
			// Start and end are in same day
			if (now >= start && now <= end)
			{
				sleeping = true;
				UpdateLastUpdatedText();
			}
			else
			{
				sleeping = false;
				Run();
			}
		}
		else
		{
			// Start and end are in different days
			if (now >= start || now <= end)
			{
				sleeping = true;
				UpdateLastUpdatedText();
			}
			else
			{
				sleeping = false;
				Run();
			}
		}
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
		string message = "Last Updated: " + DateTime.Now.ToString("HH:mm");

		if (sleeping)
		{
			TimeSpan sleepEndTime = TimeSpan.Parse(sleepEnd);
			TimeSpan now = DateTime.Now.TimeOfDay;
			TimeSpan repeatRateTime = TimeSpan.FromSeconds(GetRepeatRateInSeconds());
			TimeSpan nextRepeatTime = now.Add(repeatRateTime);

			while (nextRepeatTime < sleepEndTime)
			{
				nextRepeatTime = nextRepeatTime.Add(repeatRateTime);
			}
			message = "Waking Up At: " + nextRepeatTime.Hours.ToString("00") + ":" + nextRepeatTime.Minutes.ToString("00");
		}

		lastUpdatedText.text = message;
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