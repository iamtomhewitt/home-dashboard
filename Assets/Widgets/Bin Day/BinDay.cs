﻿using UnityEngine;
using UnityEngine.UI;
using System;
using SimpleJSON;

namespace BinDay
{
	public class BinDay : Widget
	{
		[Header("Bin Day Settings")]
		[SerializeField] private Color greenBinColour;
		[SerializeField] private Color blackBinColour;
		[SerializeField] private Color noBinColour;

		[SerializeField] private Text text;
		[SerializeField] private Image logo;

		private DateTime firstGreenBinDay;
		private DateTime firstBlackBinDay;

		private int repeatRateInDays;

		private void Start()
		{
			this.ReloadConfig();
			this.Initialise();
			InvokeRepeating("Run", 0f, RepeatRateInSeconds());
		}

		public override void ReloadConfig()
		{
			JSONNode config = Config.instance.GetConfig()[this.GetWidgetConfigKey()]["binDay"];
			firstGreenBinDay = DateTime.ParseExact(config["firstGreenBin"], "dd-MM-yyyy", null);
			firstBlackBinDay = DateTime.ParseExact(config["firstBlackBin"], "dd-MM-yyyy", null);
			repeatRateInDays = config["repeatRateInDays"];
		}

		public override void Run()
		{
			this.ReloadConfig();

			DateTime today = DateTime.Today;
			DateTime tomorrow = today.AddDays(1);

			DateTime lastGreenBinDay = GetLastBinDate(firstGreenBinDay);
			DateTime lastBlackBinDay = GetLastBinDate(firstBlackBinDay);
			DateTime nextGreenBinDay = GetNextBinDate(lastGreenBinDay);
			DateTime nextBlackBinDay = GetNextBinDate(lastBlackBinDay);

			if (today == nextBlackBinDay || today == lastBlackBinDay)
			{
				Display("Black bin today!", FontStyle.Bold, blackBinColour, Lighten(blackBinColour));
			}
			else if (today == nextGreenBinDay || today == lastGreenBinDay)
			{
				Display("Green bin today!", FontStyle.Bold, greenBinColour, Darken(greenBinColour));
			}
			else if (tomorrow == nextBlackBinDay || tomorrow == lastBlackBinDay)
			{
				Display("Black bin tomorrow!", FontStyle.Normal, blackBinColour, Lighten(blackBinColour));
			}
			else if (tomorrow == nextGreenBinDay || tomorrow == lastGreenBinDay)
			{
				Display("Green bin tomorrow!", FontStyle.Normal, greenBinColour, Darken(greenBinColour));
			}
			else
			{
				Display("No bins today!", FontStyle.Normal, noBinColour, Darken(noBinColour));
			}

			this.UpdateLastUpdatedText();
		}

		private void Display(string message, FontStyle style, Color widgetColour, Color logoColour)
		{
			text.text = message;
			text.fontStyle = style;
			logo.color = logoColour;
			this.SetColour(widgetColour);
		}

		private Color Darken(Color colour)
		{
			float multiplier = 0.75f;
			return new Color(colour.r * multiplier, colour.g * multiplier, colour.b * multiplier, 1f);
		}

		private Color Lighten(Color colour)
		{
			float multiplier = 2f;
			return new Color(colour.r * multiplier, colour.g * multiplier, colour.b * multiplier, 1f);
		}

		private DateTime GetLastBinDate(DateTime firstBinDate)
		{
			DateTime today = DateTime.Today;
			int days = (int)(today - firstBinDate).TotalDays;
			int remainder = days % repeatRateInDays;
			return today.AddDays(-remainder);
		}

		private DateTime GetNextBinDate(DateTime lastBinDate)
		{
			return lastBinDate.AddDays(14);
		}
	}
}