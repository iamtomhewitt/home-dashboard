using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using SimpleJSON;
using Dialog;
using Requests;
using System;

namespace Train
{
	public class Trains : FadingWidget
	{
		[Header("Train Settings")]
		[SerializeField] private TrainEntry[] trainEntries;
		private JSONNode json;

		private string apiToken;
		private string stationCode;

		private int maxDestinationLength = 10;

		private void Start()
		{
			apiToken 	= Config.instance.GetConfig()["apiKeys"]["trains"];
			stationCode = Config.instance.GetConfig()["trains"]["stationCode"];
			
			this.Initialise();
			InvokeRepeating("Run", 0f, RepeatRateInSeconds());
		}

		public override void Run()
		{
			apiToken = Config.instance.GetConfig()["apiKeys"]["trains"];
			stationCode = Config.instance.GetConfig()["trains"]["stationCode"];

			StartCoroutine(Fade(PopulateEntries, 1f));
			this.UpdateLastUpdatedText();
		}

		private IEnumerator PopulateEntries()
		{
			foreach (TrainEntry entry in trainEntries)
			{
				entry.GetDestinationText().text = "";
				entry.GetTimeText().text = "";
			}

			int numberOfResults = trainEntries.Length;

			UnityWebRequest request = Postman.CreateGetRequest(Endpoints.TRAIN_DEPARTURES(stationCode, numberOfResults, apiToken));
			yield return request.SendWebRequest();
			string response = request.downloadHandler.text;

			json = JSON.Parse(response);

			bool ok = request.error == null ? true : false;
			if (!ok)
			{
				WidgetLogger.instance.Log(this, "Error: " + request.error);
				yield break;
			}

			for (int i = 0; i< json["timetable"].Count; i++)
			{
				if (json["timetable"].Count == i)
				{
					yield break;
				}

				TrainEntry entry = trainEntries[i];
				JSONNode trainService = json["timetable"][i];

				string locationName = trainService["destination"];
				string scheduledDepartTime = DateTime.Parse(trainService["scheduledDepartTime"]).ToString("HH:mm");
				string actualDepartTime = trainService["actualDepartTime"] == null ? scheduledDepartTime : DateTime.Parse(trainService["actualDepartTime"]).ToString("HH:mm");

				if (actualDepartTime.Equals(scheduledDepartTime))
				{
					actualDepartTime = "On time";
				}

				if (locationName.Length > maxDestinationLength)
				{
					locationName = locationName.Substring(0, maxDestinationLength - 1) + "...";
				}

				entry.GetDestinationText().text = locationName;
				entry.GetTimeText().text = scheduledDepartTime + " (" + actualDepartTime + ")";
			}







			//int lastRowNumber = 0;

			//for (int i = 0; i < json["trainServices"].Count; i++)
			//{
			//	if (json["trainServices"].Count == i)
			//	{
			//		yield break;
			//	}

			//	TrainEntry entry = trainEntries[i];
			//	JSONNode trainService = json["trainServices"][i];
			//	string locationName = trainService["destination"][0]["locationName"];

			//	if (locationName.Length > maxDestinationLength)
			//	{
			//		locationName = locationName.Substring(0, maxDestinationLength - 1) + "...";
			//	}

			//	entry.GetDestinationText().text = locationName;
			//	entry.GetTimeText().text = trainService["std"] + " (" + trainService["etd"] + ")";

			//	lastRowNumber++;
			//}

			//for (int i = 0; i < json["busServices"].Count; i++)
			//{
			//	if (json["busServices"].Count == i)
			//	{
			//		yield break;
			//	}

			//	TrainEntry entry = trainEntries[lastRowNumber];
			//	JSONNode trainService = json["busServices"][i];
			//	string locationName = trainService["destination"][0]["locationName"];

			//	if (locationName.Length > maxDestinationLength)
			//	{
			//		locationName = locationName.Substring(0, maxDestinationLength - 1) + "...";
			//	}

			//	entry.GetDestinationText().text = locationName;
			//	entry.GetTimeText().text = trainService["std"] + " (Bus)";
			//	lastRowNumber++;
			//}
		}
	}
}
