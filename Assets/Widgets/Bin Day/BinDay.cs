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

			foreach(JSONNode bin in bins)
			{
				print (bin);
			}
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