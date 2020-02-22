using UnityEngine;
using UnityEngine.UI;

namespace JourneyPlanner
{
	public class JourneyPlannerEntry : MonoBehaviour
	{
		[SerializeField] private Text journeyName;
		[SerializeField] private Text journeyTime;
		[SerializeField] private Image icon;

		public void Initialise(string journeyName, string journeyTime, Color iconColour)
		{
			this.journeyName.text = journeyName;
			this.journeyTime.text = journeyTime;
			this.icon.color = iconColour;
		}
	}
}