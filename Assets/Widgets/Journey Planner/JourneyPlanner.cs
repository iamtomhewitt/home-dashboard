using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using Requests;
using SimpleJSON;

namespace JourneyPlanner
{
	public class JourneyPlanner : Widget
	{
		[Header("Journey Planner Settings")]
		[SerializeField] private Transform scrollContent;
		[SerializeField] private JourneyPlannerEntry scrollEntry;
		[SerializeField] private JourneyPlannerEntry singleEntry;
		[SerializeField] private Image scrollbarBackground;
		[SerializeField] private Image scrollbarHandle;
		[SerializeField] private Color heavyTrafficColour;
		[SerializeField] private Color mediumTrafficColour;
		[SerializeField] private Color noTrafficColour;

		private JSONNode journeys;
		private string apiKey;
		private const int FIFTEEN_MINS = 900; 
		private const int THIRTY_MINS = 1800; 

		public override void ReloadConfig()
		{
			JSONNode config = Config.instance.GetWidgetConfig()[this.GetWidgetConfigKey()];
			apiKey = config["apiKey"];
			journeys = config["journeys"];
			scrollbarBackground.color = Colours.Darken(GetWidgetColour());
			scrollbarHandle.color = Colours.Lighten(GetWidgetColour());
		}

		public override void Run()
		{
			this.ReloadConfig();

			StartCoroutine(RunRoutine());

			this.UpdateLastUpdatedText();
		}

		private IEnumerator RunRoutine()
		{
			foreach (JSONNode journey in journeys)
			{
				UnityWebRequest request = Postman.CreateGetRequest(Endpoints.JOURNEY_PLANNER(journey["startPoint"], journey["endPoint"], apiKey));
				yield return request.SendWebRequest();
				JSONNode json = JSON.Parse(request.downloadHandler.text);

				int duration			= json["resourceSets"][0]["resources"][0]["travelDuration"];
				int durationWithTraffic = json["resourceSets"][0]["resources"][0]["travelDurationTraffic"];
				int timeDifference = durationWithTraffic - duration;

				Color iconColour = noTrafficColour;

				if (timeDifference > FIFTEEN_MINS && timeDifference < THIRTY_MINS)
				{
					iconColour = mediumTrafficColour;
				}
				else if (timeDifference > THIRTY_MINS)
				{
					iconColour = heavyTrafficColour;
				}

				GameObject prefab = journeys.Count == 1 ? singleEntry.gameObject : scrollEntry.gameObject;
				Transform parent = journeys.Count == 1 ? this.transform : scrollContent;
				Instantiate(prefab, parent).GetComponent<JourneyPlannerEntry>().Initialise(journey["name"], ConvertToTimeString(durationWithTraffic), iconColour);
			}
		}

		/// <summary>
		/// Passing 3468 seconds will return "57 minutes"
		/// </summary>
		private string ConvertToTimeString(int durationInSeconds)
		{
			string timeString = "";
			
			int day = durationInSeconds / (24 * 3600);

			durationInSeconds = durationInSeconds % (24 * 3600);
			int hour = durationInSeconds / 3600;

			durationInSeconds %= 3600;
			int minutes = durationInSeconds / 60;

			durationInSeconds %= 60;
			int seconds = durationInSeconds;

			if (day > 0)
			{
				timeString += day + "d ";
			}

			if (hour > 0)
			{
				timeString += hour + "h ";
			}

			if (minutes > 0)
			{
				timeString += minutes + "m ";
			}

			return timeString;
		}
	}
}
