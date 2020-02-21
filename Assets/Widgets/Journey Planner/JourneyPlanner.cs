using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using Requests;
using SimpleJSON;

public class JourneyPlanner : Widget
{
	private JSONNode config;
	private JSONNode journeys;
	private string apiKey;

	public override void ReloadConfig()
	{
		config = Config.instance.GetWidgetConfig()[this.GetWidgetConfigKey()];
		apiKey = config["apiKey"];
		journeys = config["journeys"];
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

			int durationNormal = json["resourceSets"][0]["resources"][0]["travelDuration"];
			int durationTraffic = json["resourceSets"][0]["resources"][0]["travelDurationTraffic"];

			print(journey["name"]);
			print(ConvertToTimeString(durationNormal));
			print(ConvertToTimeString(durationTraffic));
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
