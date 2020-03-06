using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace JourneyPlanner
{
	public class JourneyPlannerEntry : MonoBehaviour
	{
		[SerializeField] private TMP_Text journeyName;
		[SerializeField] private TMP_Text journeyTime;
		[SerializeField] private Image icon;

		public void Initialise(string journeyName, string journeyTime, Color iconColour)
		{
			this.journeyName.text = journeyName;
			this.journeyTime.text = journeyTime;
			this.icon.color = iconColour;
		}
	}
}