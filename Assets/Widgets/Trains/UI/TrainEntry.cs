using UnityEngine;
using UnityEngine.UI;

namespace Train
{
	public class TrainEntry : MonoBehaviour
	{
		[SerializeField] private Text destinationText;
		[SerializeField] private Text timeText;

		public void SetDestinationText(string text)
		{
			destinationText.text = text;
		}

		public void SetTimeText(string text)
		{
			timeText.text = text;
		}

		public void SetTextColour(Color colour)
		{
			destinationText.color = colour;
			timeText.color = colour;
		}

		public override string ToString()
		{
			return string.Format("Destination: {0}, Time: {1}", destinationText.text, timeText.text);
		}
	}
}