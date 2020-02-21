﻿using UnityEngine;
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
		[SerializeField] private JourneyPlannerEntry entry;
		[SerializeField] private Image scrollbarBackground;
		[SerializeField] private Image scrollbarHandle;

		private JSONNode config;
		private JSONNode journeys;
		private string apiKey;

		public override void ReloadConfig()
		{
			config = Config.instance.GetWidgetConfig()[this.GetWidgetConfigKey()];
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

				int durationWithTraffic = json["resourceSets"][0]["resources"][0]["travelDurationTraffic"];

				Instantiate(entry, scrollContent).GetComponent<JourneyPlannerEntry>().Initialise(journey["name"], ConvertToTimeString(durationWithTraffic));
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
				timeString += day + " days ";
			}

			if (hour > 0)
			{
				timeString += hour + " hours ";
			}

			if (minutes > 0)
			{
				timeString += minutes + " minutes";
			}

			return timeString;
		}
	}
}
