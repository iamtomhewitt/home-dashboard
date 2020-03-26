using UnityEngine;
using TMPro;
using Dialog;

namespace GoogleCalendar
{
	public class GoogleCalendarEvent : MonoBehaviour
	{
		[SerializeField] private TMP_Text summaryText;
		[SerializeField] private TMP_Text startDateText;

		private GoogleCalendar calendar;
		private string endDate;
		private string startTime;
		private string endTime;
		private string location;
		private string description;

		public void SetSummaryText(string text)
		{
			summaryText.text = text;
		}

		public void SetStartDateText(string text)
		{
			startDateText.text = text;
		}

		public void SetEndDateText(string text)
		{
			endDate = text;
		}

		public void SetSummaryTextColour(Color colour)
		{
			summaryText.color = colour;
		}

		public void SetStartDateTextColour(Color colour)
		{
			startDateText.color = colour;
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

		public void SetGoogleCalendar(GoogleCalendar calendar)
		{
			this.calendar = calendar;
		}

		public GoogleCalendar GetGoogleCalendar()
		{
			return calendar;
		}

		/// <summary>
		/// Called from a button.
		/// </summary>
		public void ShowEventDetails()
		{
			GoogleCalendarEventDialog dialog = FindObjectOfType<GoogleCalendarEventDialog>();
			dialog.SetParentWidget(calendar);
			dialog.SetDialogTitleText(summaryText.text);
			dialog.Populate(startDateText.text, endDate, startTime, endTime, location, description);
			dialog.ApplyColours();
			dialog.Show();
		}
	}
}