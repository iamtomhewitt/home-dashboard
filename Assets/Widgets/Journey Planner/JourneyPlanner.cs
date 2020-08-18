using Dialog;
using Requests;
using SimpleJSON;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine;

namespace JourneyPlanner
{
	public class JourneyPlanner : Widget
	{
		[Header("Journey Planner Settings")]
		[SerializeField] private Color heavyTrafficColour;
		[SerializeField] private Color mediumTrafficColour;
		[SerializeField] private Color noTrafficColour;
		[SerializeField] private Image scrollbarBackground;
		[SerializeField] private Image scrollbarHandle;
		[SerializeField] private JourneyPlannerEntry scrollEntry;
		[SerializeField] private JourneyPlannerEntry singleEntry;
		[SerializeField] private Transform scrollContent;

		private JSONNode journeys;
		private int heavyTrafficMinutes;
		private int mediumTrafficMinutes;

		public override void ReloadConfig()
		{
			JSONNode config = Config.instance.GetWidgetConfig()[this.GetWidgetConfigKey()];
			journeys = config["journeys"];
			mediumTrafficMinutes = config["mediumTrafficMinutes"] == null ? 15 : config["mediumTrafficMinutes"].AsInt;
			heavyTrafficMinutes = config["heavyTrafficMinutes"] == null ? 25 : config["heavyTrafficMinutes"].AsInt;
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
			// Clear out existing entries
			foreach (Transform child in scrollContent)
			{
				Destroy(child.gameObject);
			}

			foreach (JSONNode journey in journeys)
			{
				List<string> stops = new List<string>();
				if (journey["stops"] != null)
				{
					foreach (JSONNode stop in journey["stops"])
					{
						stops.Add(stop);
					}
				}

				UnityWebRequest request = Postman.CreateGetRequest(Endpoints.instance.JOURNEY_PLANNER(journey["startPoint"], stops, journey["endPoint"]));
				yield return request.SendWebRequest();

				bool ok = request.error == null ? true : false;
				if (!ok)
				{
					WidgetLogger.instance.Log(this, "Error: " + request.error);
					yield break;
				}

				JSONNode json = JSON.Parse(request.downloadHandler.text);

				int duration = json["resourceSets"][0]["resources"][0]["travelDuration"];
				int durationWithTraffic = json["resourceSets"][0]["resources"][0]["travelDurationTraffic"];
				int timeDifference = (durationWithTraffic - duration) / 60;

				Color trafficColour = noTrafficColour;

				if (timeDifference > mediumTrafficMinutes && timeDifference < heavyTrafficMinutes)
				{
					trafficColour = mediumTrafficColour;
				}
				else if (timeDifference > heavyTrafficMinutes)
				{
					trafficColour = heavyTrafficColour;
				}

				GameObject prefab = journeys.Count == 1 ? singleEntry.gameObject : scrollEntry.gameObject;
				Transform parent = journeys.Count == 1 ? this.transform : scrollContent;
				Instantiate(prefab, parent).GetComponent<JourneyPlannerEntry>().Initialise(journey["name"], ConvertToTimeString(durationWithTraffic), trafficColour);
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
