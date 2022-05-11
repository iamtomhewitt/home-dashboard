using TMPro;
using UnityEngine;

namespace Bus
{
	public class BusEntry : MonoBehaviour
	{
		[SerializeField] private TMP_Text routeNumberText;
		[SerializeField] private TMP_Text destinationText;
		[SerializeField] private TMP_Text expectedText;

		public void SetDestinationText(string text)
		{
			destinationText.text = text;
		}

		public void SetRouteNumberText(string text)
		{
			routeNumberText.text = text;
		}

		public void SetExpectedText(string text)
		{
			expectedText.text = text;
		}

		public void SetTextColour(Color colour)
		{
			destinationText.color = colour;
			expectedText.color = colour;
			routeNumberText.color = colour;
		}

		public override string ToString()
		{
			return string.Format("Destination: {0}, Route Number: {1}, Expected: {2}", destinationText.text, routeNumberText.text, expectedText.text);
		}
	}
}