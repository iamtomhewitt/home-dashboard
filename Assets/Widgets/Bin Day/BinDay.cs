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

		private void Start()
		{
			this.Initialise();
			InvokeRepeating("Run", 0f, RepeatRateInSeconds());
		}

		public override void Run()
		{
			DateTime firstGreenBinDay = new DateTime(2019, 10, 7);
			DateTime firstBlackBinDay = new DateTime(2019, 10, 14);
			DateTime today = DateTime.Today;
			DateTime tomorrow = today.AddDays(1);

			int greenBinDays = (int)(today - firstGreenBinDay).TotalDays;
			int blackBinDays = (int)(today - firstBlackBinDay).TotalDays;

			int greenBinRemainder = greenBinDays % 14;
			int blackBinRemainder = blackBinDays % 14;

			DateTime lastGreenBinDay = today.AddDays(-greenBinRemainder);
			DateTime nextGreenBinDay = lastGreenBinDay.AddDays(14);

			DateTime lastBlackBinDay = today.AddDays(-blackBinRemainder);
			DateTime nextBlackBinDay = lastBlackBinDay.AddDays(14);

			print("Today: " + today.ToString("dd/MM/yyyy"));
			print("Next green bin: " + nextGreenBinDay.ToString("dd/MM/yyyy"));
			print("Next black bin: " + nextBlackBinDay.ToString("dd/MM/yyyy"));

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
	}
}