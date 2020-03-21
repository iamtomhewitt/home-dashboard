using TMPro;
using UnityEngine;

namespace Dialog
{
	/// <summary>
	/// A dialog that adds an item to an online list.
	/// </summary>
	public class GoogleCalendarEventDialog : PopupDialog
	{
		[SerializeField] private TMP_Text summaryText;
		[SerializeField] private TMP_Text startDateText;
		[SerializeField] private TMP_Text startTimeText;
		[SerializeField] private TMP_Text endDateText;
		[SerializeField] private TMP_Text endTimeText;
		[SerializeField] private TMP_Text locationText;
		[SerializeField] private TMP_Text descriptionText;

		public override void ApplyAdditionalColours(Color mainColour, Color textColour)
		{
			summaryText.color = textColour;
			startDateText.color = textColour;
			startTimeText.color = textColour;
			endDateText.color = textColour;
			endTimeText.color = textColour;
		}

		public void Populate(string summary, string startDate, string endDate, string startTime, string endTime, string location, string description)
		{
			summaryText.text = summary;
			startDateText.text = startDate;
			startTimeText.text = startTime;
			endDateText.text = endDate;
			endTimeText.text = endTime;
			locationText.text = location;
			descriptionText.text = description;
		}
	}
}