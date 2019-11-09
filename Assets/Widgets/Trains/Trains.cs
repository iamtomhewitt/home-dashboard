using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using JsonResponse;

namespace Train
{
	public class Trains : Widget
	{
		[Space(15f)]
		[SerializeField] private TrainEntry[] trainEntries;
		[SerializeField] private Config config;

		private string apiToken;
		private string stationCode = "HRS";

		private int numberOfResults = 5;
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

			string url = "https://huxley.apphb.com/departures/" + stationCode + "/" + numberOfResults + "?accessToken=" + apiToken;

			UnityWebRequest request = UnityWebRequest.Get(url);
			yield return request.SendWebRequest();
			string jsonResponse = request.downloadHandler.text;

			TrainJsonResponse trainData = JsonUtility.FromJson<TrainJsonResponse>(jsonResponse);

			for (int i = 0; i < trainData.trainServices.Length; i++)
			{
				if (trainData.trainServices.Length == i)
				{
					yield break;
				}

				TrainEntry entry = trainEntries[i];
				TrainServices trainService = trainData.trainServices[i];
				string locationName = trainService.destination[0].locationName;

				if (locationName.Length > maxDestinationLength)
				{
					locationName = locationName.Substring(0, maxDestinationLength -1) + "...";
				}

				entry.GetDestinationText().text = locationName;
				entry.GetTimeText().text = trainService.std + " (" + trainService.etd + ")";
			}
		}
	}
}
