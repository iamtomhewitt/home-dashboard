using UnityEngine;
using UnityEngine.UI;
using System;
using SimpleJSON;

namespace Bins
{
	public class BinDay : Widget
	{
		[Header("Bin Day Settings")]
		[SerializeField] private Color noBinColour;

		[SerializeField] private Text text;
		[SerializeField] private Image logo;

		private JSONArray bins;

		public override void ReloadConfig()
		{
			JSONNode config = Config.instance.GetWidgetConfig()[this.GetWidgetConfigKey()];
			noBinColour = Colours.ToColour(config["noBinColour"]);
			bins = config["bins"].AsArray;
		}

		public override void Run()
		{
			this.ReloadConfig();

			DateTime today = DateTime.Today;
			DateTime tomorrow = today.AddDays(1);

			bool stopProcessing = false;

			foreach(JSONNode bin in bins)
			{
				// If the first bin in the config is active, and we carry on processing, 
				// then the next time around the widget will show 'no bins today' 
				if (stopProcessing)
				{
					break;
				}

				DateTime firstBinDay = DateTime.ParseExact(bin["firstDate"], "dd-MM-yyyy", null);
				int repeatRateInDays = bin["repeatRateInDays"];
				string binName = bin["name"];

				DateTime lastBinDay = GetLastBinDate(firstBinDay, repeatRateInDays);
				DateTime nextBinDay = GetNextBinDate(lastBinDay, repeatRateInDays);

				Color binColour = Colours.ToColour(bin["binColour"]);

				if (IsBin(today, nextBinDay, lastBinDay))
				{
					Display(binName + " bin today!", FontStyle.Bold, binColour, Colours.Lighten(binColour));
					stopProcessing = true;
				}
				else if (IsBin(tomorrow, nextBinDay, lastBinDay))
				{
					Display(binName + " bin tomorrow!", FontStyle.Normal, binColour, Colours.Lighten(binColour));
					stopProcessing = true;
				}
				else
				{
					Display("No bins today!", FontStyle.Normal, noBinColour, Colours.Darken(noBinColour));
				}
			}

			this.UpdateLastUpdatedText();
		}

		private bool IsBin(DateTime date, DateTime next, DateTime last)
		{
			return (next == date || last == date);
		}

		private void Display(string message, FontStyle style, Color widgetColour, Color logoColour)
		{
			text.text = message;
			text.color = GetTextColour();
			text.fontStyle = style;
			logo.color = logoColour;
			this.SetWidgetColour(widgetColour);
		}

		private DateTime GetLastBinDate(DateTime firstBinDate, int repeatRateInDays)
		{
			DateTime today = DateTime.Today;
			int days = (int)(today - firstBinDate).TotalDays;
			int remainder = days % repeatRateInDays;
			return today.AddDays(-remainder);
		}

		private DateTime GetNextBinDate(DateTime lastBinDate, int repeatRateInDays)
		{
			return lastBinDate.AddDays(repeatRateInDays);
		}
	}
}