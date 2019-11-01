using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Train
{
	public class Trains : Widget
	{
		[Space(15f)]
		[SerializeField] private TrainEntry[] trainEntries;

		private string apiToken = "4ae0f545-388c-4812-a4c9-b72ffb815abd";
		private string stationCode = "HRS";

		private int numberOfResults = 5;

		private void Start()
		{
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
			TrainServices[] services = trainData.trainServices;

			for (int i = 0; i < services.Length; i++)
			{
				if (services.Length == i)
				{
					yield break;
				}

				TrainEntry entry = trainEntries[i];
				TrainServices trainService = trainData.trainServices[i];
				entry.GetDestinationText().text = trainService.destination[0].locationName;
				entry.GetTimeText().text = trainService.std + " (" + trainService.etd + ")";
			}
		}
	}
}
