using UnityEngine;
using UnityEngine.UI;

namespace JourneyPlanner
{
	public class JourneyPlannerEntry : MonoBehaviour
	{
		[SerializeField] private Text journeyName;
		[SerializeField] private Text journeyTime;

		public void Initialise(string journeyName, string journeyTime, Color colour)
		{
			this.journeyName.text = journeyName;
			this.journeyTime.text = journeyTime;

			this.journeyName.color = colour;
			this.journeyTime.color = colour;
		}
	}
}