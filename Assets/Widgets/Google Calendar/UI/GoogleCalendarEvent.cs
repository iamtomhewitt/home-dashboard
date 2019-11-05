using UnityEngine;
using UnityEngine.UI;

namespace GoogleCalendar
{
	public class GoogleCalendarEvent : MonoBehaviour
	{
		[SerializeField] private Text nameText;
		[SerializeField] private Text dateText;

		public Text GetNameText()
		{
			return nameText;
		}

		public Text GetDateText()
		{
			return dateText;
		}
	}
}