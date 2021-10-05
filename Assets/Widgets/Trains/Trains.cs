using Dialog;
using Requests;
using SimpleJSON;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine;

namespace Train
{
	public class Trains : FadingWidget
	{
		[Header("Train Settings")]
		[SerializeField] private TrainEntry[] trainEntries;
		[SerializeField] private Image scrollbarBackground;
		[SerializeField] private Image scrollbarHandle;

		private int maxDestinationLength = 10;

		public override void ReloadConfig() { }

		public override void Run()
		{
			this.ReloadConfig();
			StartCoroutine(Fade(RunRoutine, 1f));
			scrollbarBackground.color = Colours.Darken(GetWidgetColour());
			scrollbarHandle.color = Colours.Lighten(GetWidgetColour());
			this.UpdateLastUpdatedText();
		}

		private IEnumerator RunRoutine()
		{
			UnityWebRequest request = Postman.CreateGetRequest(Endpoints.instance.TRAIN_DEPARTURES());
			yield return request.SendWebRequest();

			JSONNode json = JSON.Parse(request.downloadHandler.text);

			bool ok = request.error == null ? true : false;
			if (!ok)
			{
				WidgetLogger.instance.Log(this, "Error: " + request.error);
				yield break;
			}

			foreach (TrainEntry entry in trainEntries)
			{
				entry.SetDestinationText("");
				entry.SetTimeText("");
			}

			for (int i = 0; i < trainEntries.Length; i++)
			{
				JSONNode data = json["departures"]["all"][i];
				if (data == null)
				{
					break;
				}

				string transport = data["mode"] == "train" ? "" : data["mode"].ToString();
				string destination = data["destination_name"];
				string departureTime = data["aimed_departure_time"];
				string expectedDepartureTime = data["expected_departure_time"];
				string timeLabel = departureTime == expectedDepartureTime ? "On time" : expectedDepartureTime;
				string label = departureTime + " (" + timeLabel + ")";

				if (destination.Length > maxDestinationLength)
				{
					destination = destination.Substring(0, maxDestinationLength - 1) + "...";
				}

				TrainEntry entry = trainEntries[i];
				entry.SetDestinationText(destination);
				entry.SetTimeText(label);
			}
		}
	}
}
