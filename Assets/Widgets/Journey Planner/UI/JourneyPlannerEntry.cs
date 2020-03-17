using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace JourneyPlanner
{
	public class JourneyPlannerEntry : MonoBehaviour
	{
		[SerializeField] private TMP_Text journeyName;
		[SerializeField] private TMP_Text journeyTime;

		public void Initialise(string journeyName, string journeyTime, Color colour)
		{
			this.journeyName.text = journeyName;
			this.journeyTime.text = journeyTime;
			this.journeyTime.color = colour;
		}
	}
}