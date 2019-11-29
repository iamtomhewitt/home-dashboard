using UnityEngine;
using UnityEngine.UI;
using System;

namespace BinDay
{
	public class BinDay : Widget
	{
		[Space(15f)]
		[SerializeField] private Color greenBinColour;
		[SerializeField] private Color blackBinColour;
		[SerializeField] private Color noBinColour;

		[SerializeField] private Text text;
		[SerializeField] private Image logo;

		private DateTime firstGreenBinDay;
		private DateTime firstBlackBinDay;

		private void Start()
		{
			firstGreenBinDay = DateTime.ParseExact(Config.instance.GetConfig()["binDay"]["firstGreenBin"], "dd-MM-yyyy", null);
			firstBlackBinDay = DateTime.ParseExact(Config.instance.GetConfig()["binDay"]["firstBlackBin"], "dd-MM-yyyy", null);

			this.Initialise();
			InvokeRepeating("Run", 0f, RepeatRateInSeconds());
		}

		public override void Run()
		{
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
			int remainder = days % 14;
			return today.AddDays(-remainder);
		}

		private DateTime GetNextBinDate(DateTime lastBinDate)
		{
			return lastBinDate.AddDays(14);
		}
	}
}