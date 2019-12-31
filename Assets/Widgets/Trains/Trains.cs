﻿using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using SimpleJSON;
using Dialog;

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

			string url = "https://huxley.apphb.com/departures/" + stationCode + "/" + numberOfResults + "?accessToken=" + apiToken;

			UnityWebRequest request = UnityWebRequest.Get(url);
			yield return request.SendWebRequest();
			string response = request.downloadHandler.text;

			json = JSON.Parse(response);

			bool ok = request.error == null ? true : false;
			if (!ok)
			{
				WidgetLogger.instance.Log(this, "Error: " + request.error);
				yield break;
			}

			int lastRowNumber = 0;

			for (int i = 0; i < json["trainServices"].Count; i++)
			{
				if (json["trainServices"].Count == i)
				{
					yield break;
				}

				TrainEntry entry = trainEntries[i];
				JSONNode trainService = json["trainServices"][i];
				string locationName = trainService["destination"][0]["locationName"];

				if (locationName.Length > maxDestinationLength)
				{
					locationName = locationName.Substring(0, maxDestinationLength - 1) + "...";
				}

				entry.GetDestinationText().text = locationName;
				entry.GetTimeText().text = trainService["std"] + " (" + trainService["etd"] + ")";

				lastRowNumber++;
			}

			for (int i = 0; i < json["busServices"].Count; i++)
			{
				if (json["busServices"].Count == i)
				{
					yield break;
				}

				TrainEntry entry = trainEntries[lastRowNumber];
				JSONNode trainService = json["busServices"][i];
				string locationName = trainService["destination"][0]["locationName"];

				if (locationName.Length > maxDestinationLength)
				{
					locationName = locationName.Substring(0, maxDestinationLength - 1) + "...";
				}

				entry.GetDestinationText().text = locationName;
				entry.GetTimeText().text = trainService["std"] + " (Bus)";
				lastRowNumber++;
			}
		}
	}
}
