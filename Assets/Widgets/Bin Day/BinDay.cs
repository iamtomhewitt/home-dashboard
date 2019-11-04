using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BinDay
{
	public class BinDay : Widget
	{
		[Space(15f)]
		[SerializeField] private Color greenBinColour;
		[SerializeField] private Color blackBinColour;
		[SerializeField] private Color noBinColour;

		[SerializeField] private Text text;

		private DateTime firstGreenBinDay = new DateTime(2019, 10, 7);
		private DateTime firstBlackBinDay = new DateTime(2019, 10, 14);

		private void Start()
		{
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
				text.text = "Black bin today!";
				text.fontStyle = FontStyle.Bold;
				this.SetColour(blackBinColour);
			}
			else if (today == nextGreenBinDay || today == lastGreenBinDay)
			{
				text.text = "Green bin today!";
				text.fontStyle = FontStyle.Bold;
				this.SetColour(greenBinColour);
			}
			else if (tomorrow == nextBlackBinDay || tomorrow == lastBlackBinDay)
			{
				text.text = "Black bin tomorrow!";
				this.SetColour(blackBinColour);
			}
			else if (tomorrow == nextGreenBinDay || tomorrow == lastGreenBinDay)
			{
				text.text = "Green bin tomorrow!";
				this.SetColour(greenBinColour);
			}
			else
			{
				text.text = "No bin alerts yet!";
				this.SetColour(noBinColour);
			}

			this.UpdateLastUpdatedText();
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