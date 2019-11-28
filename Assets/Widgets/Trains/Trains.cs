using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using SimpleJSON;
using Dialog;

namespace Train
{
	public class Trains : Widget
	{
		[Space(15f)]
		[SerializeField] private TrainEntry[] trainEntries;
		[SerializeField] private Config config;

		private JSONNode json;

		private string apiToken;
		private string stationCode = "HRS";

		private int maxDestinationLength = 10;

		private void Start()
		{
			apiToken = config.GetConfig()["apiKeys"]["trains"];
			
			this.Initialise();
			InvokeRepeating("Run", 0f, RepeatRateInSeconds());
		}

		public override void Run()
		{
			StartCoroutine(RunRoutine());
			this.UpdateLastUpdatedText();
		}

		private IEnumerator RunRoutine()
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
			}

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
					locationName = locationName.Substring(0, maxDestinationLength -1) + "...";
				}

				entry.GetDestinationText().text = locationName;
				entry.GetTimeText().text = trainService["std"] + " (" + trainService["etd"] + ")";
			}
		}
	}
}
