using TMPro;
using UnityEngine;

namespace Dialog
{
	/// <summary>
	/// A dialog that adds an item to an online list.
	/// </summary>
	public class GoogleCalendarEventDialog : WidgetDialog
	{
		[SerializeField] private TMP_Text startDateTimeText;
		[SerializeField] private TMP_Text endDateTimeText;
		[SerializeField] private TMP_Text locationText;
		[SerializeField] private TMP_Text descriptionText;

		public override void ApplyAdditionalColours(Color mainColour, Color textColour)
		{
			startDateTimeText.color = textColour;
			endDateTimeText.color = textColour;
		}

		public void Populate(string startDate, string endDate, string startTime, string endTime, string location, string description)
		{
			startDateTimeText.text = startDate;
			endDateTimeText.text = endDate;
			locationText.text = location;
			descriptionText.text = description;
		}
	}
}