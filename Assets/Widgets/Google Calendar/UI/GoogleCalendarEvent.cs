using UnityEngine;
using UnityEngine.UI;

namespace GoogleCalendar
{
	public class GoogleCalendarEvent : MonoBehaviour
	{
		[SerializeField] private Text nameText;
		[SerializeField] private Text dateText;

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