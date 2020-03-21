using UnityEngine;
using TMPro;

namespace GoogleCalendar
{
	public class GoogleCalendarEvent : MonoBehaviour
	{
		[SerializeField] private TMP_Text summaryText;
		[SerializeField] private TMP_Text dateText;

		private string startTime;
		private string endTime;
		private string location;
		private string description;

		public void SetSummaryText(string text)
		{
			summaryText.text = text;
		}

		public void SetDateText(string text)
		{
			dateText.text = text;
		}

		public void SetNameTextColour(Color colour)
		{
			summaryText.color = colour;
		}

		public void SetDateTextColour(Color colour)
		{
			dateText.color = colour;
		}

		public void SetStartTime(string startTime)
		{
			this.startTime = startTime;
		}

		public void SetEndTime(string endTime)
		{
			this.endTime = endTime;
		}

		public void SetLocation(string location)
		{
			this.location = location;
		}

		public void SetDescription(string description)
		{
			this.description = description;
		}

		/// <summary>
		/// Called from a button.
		/// </summary>
		public void ShowEventDetails()
		{
			print(this.ToString());
		}
	}
}