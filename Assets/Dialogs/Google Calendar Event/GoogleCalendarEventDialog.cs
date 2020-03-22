using UnityEngine;
using TMPro;

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
			// Nothing to do!
		}

		public void Populate(string startDate, string endDate, string startTime, string endTime, string location, string description)
		{
			startDateTimeText.text = string.Format("<b>Start: </b> {0} {1}", startDate, startTime);
			endDateTimeText.text = string.Format("<b>End:   </b> {0} {1}", endDate, endTime);
			locationText.text = "<b>At: </b>" + location;
			descriptionText.text = "<b>Notes: </b>\n" + description;
		}
	}
}