using UnityEngine;
using UnityEngine.UI;

namespace JourneyPlanner
{
	public class JourneyPlannerEntry : MonoBehaviour
	{
		[SerializeField] private Text journeyName;
		[SerializeField] private Text journeyTime;

		public void Initialise(string journeyName, string journeyTime)
		{
			this.journeyName.text = journeyName;
			this.journeyTime.text = journeyTime;
		}
	}
}