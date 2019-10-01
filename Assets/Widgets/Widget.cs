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

	public float repeatRate;

	public abstract void Run();

	public void Initialise()
	{
		UpdateLastUpdatedText();
		SetTitleText();
		SetColour();
		SetTextColour();
	}

	private void UpdateLastUpdatedText()
	{
		lastUpdatedText.text = "Last Updated: " + DateTime.Now.ToString("HH:mm");
	}

	private void SetTitleText()
	{
		titleText.text = title;
	}

	private void SetColour()
	{
		widgetBackground.color = widgetColour;
	}

	private void SetTextColour()
	{
		titleText.color = textColour;
		lastUpdatedText.color = textColour;
	}
}
