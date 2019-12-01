using UnityEngine;
using UnityEngine.UI;
using System;

namespace Dialog
{
	public class WidgetLogger : Dialog
	{
		[SerializeField] private Text log;

		public static WidgetLogger instance;

		private DateTime lastDate;

		private void Start()
		{
			instance = this;
			lastDate = DateTime.Today;

			InvokeRepeating("HouseKeep", 0f, 10800f); // Repeat every three hours

			Hide();
		}

		public void Log(string message)
		{
			string date = DateTime.Now.ToString("dd/MMM/yy HH:mm");

			log.text += date + " | " + message + "\n";
			print(date + " | " + message);
		}

		public void Log(Widget widget, string message)
		{
			string date = DateTime.Now.ToString("dd/MMM/yy HH:mm");
			string messageFormatted = "<b><color=#" + ColorUtility.ToHtmlStringRGB(widget.GetWidgetColour()) + ">" + widget.GetWidgetTitle() + "</color></b>: " + message;

			log.text += date + " | " + messageFormatted + "\n";
			print(date + " | " + messageFormatted);
		}

		private void HouseKeep()
		{
			if (lastDate != DateTime.Today)
			{
				log.text = "";
			}

			lastDate = DateTime.Today;
		}
	}
}