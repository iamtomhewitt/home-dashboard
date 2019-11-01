using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BinDay
{
	public class BinDay : Widget
	{
		[Space(15f)]
		[SerializeField] private List<int> greenBinDays;
		[SerializeField] private List<int> blackBinDays;

		[Space()]
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
}