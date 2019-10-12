using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BinDay : Widget
{
	[Space(15f)]
	public List<int> greenBinDays;
	public List<int> blackBinDays;

	[Space()]
	public Color greenBinColour;
	public Color blackBinColour;
	public Color noBinColour;

	public Text text;

	private void Start()
	{
		this.Initialise();
		InvokeRepeating("Run", 0f, ToSeconds());
	}

	public override void Run()
	{
		DateTime now = DateTime.Now;
		DateTime tomorrow = now.AddDays(1);

		if (greenBinDays.Contains(now.Day))
		{
			text.text = "Green bin today!";
			text.fontStyle = FontStyle.Bold;
			this.SetColour(greenBinColour);
		}
		else if (greenBinDays.Contains(tomorrow.Day))
		{
			text.text = "Green bin tomorrow!";
			this.SetColour(greenBinColour);
		}

		else if (blackBinDays.Contains(now.Day))
		{
			text.text = "Black bin today!";
			text.fontStyle = FontStyle.Bold;
			this.SetColour(blackBinColour);
		}
		else if (blackBinDays.Contains(tomorrow.Day))
		{
			text.text = "Black bin tomorrow!";
			this.SetColour(blackBinColour);
		}

		else
		{
			text.text = "No bin alerts yet!";
			this.SetColour(noBinColour);
		}

		this.UpdateLastUpdatedText();
	}
}
