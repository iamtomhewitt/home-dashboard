using UnityEngine;
using TMPro;

namespace GoogleCalendar
{
	public class GoogleCalendarEvent : MonoBehaviour
	{
		[SerializeField] private TMP_Text nameText;
		[SerializeField] private TMP_Text dateText;

		public void SetNameText(string text)
		{
			nameText.text = text;
		}

		public void SetDateText(string text)
		{
			dateText.text = text;
		}

		public void SetNameTextColour(Color colour)
		{
			nameText.color = colour;
		}

		public void SetDateTextColour(Color colour)
		{
			dateText.color = colour;
		}
	}
}