using UnityEngine;
using UnityEngine.UI;

namespace Train
{
	public class TrainEntry : MonoBehaviour
	{
		[SerializeField] private Text destinationText;
		[SerializeField] private Text timeText;

		public Text GetDestinationText()
		{
			return destinationText;
		}

		public Text GetTimeText()
		{
			return timeText;
		}
	}
}