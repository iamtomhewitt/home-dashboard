using Dialog;
using Requests;
using SimpleJSON;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine;

namespace Bus
{
	/// <summary>
	/// Note, this is currently only to work with NCTX Buses!
	/// </summary>
	public class Buses : FadingWidget
	{
		[Header("Bus Settings")]
		[SerializeField] private BusEntry[] busEntries;
		[SerializeField] private Image scrollbarBackground;
		[SerializeField] private Image scrollbarHandle;

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
			UnityWebRequest request = Postman.CreateGetRequest(Endpoints.instance.BUS_DEPARTURES());
			yield return request.SendWebRequest();

			JSONNode json = JSON.Parse(request.downloadHandler.text);

			bool ok = request.error == null ? true : false;
			if (!ok)
			{
				WidgetLogger.instance.Log(this, "Error: " + request.error);
				yield break;
			}

			foreach (BusEntry entry in busEntries)
			{
				entry.SetDestinationText("");
				entry.SetRouteNumberText("");
				entry.SetExpectedText("");
			}

			for (int i = 0; i < busEntries.Length; i++)
			{
				JSONNode data = json["departures"][i];
				if (data == null)
				{
					break;
				}

				string routeNumber = data["routeNumber"];
				string destination = data["destination"];
				string expected = data["expected"];

				BusEntry entry = busEntries[i];
				entry.SetDestinationText(destination);
				entry.SetExpectedText(expected);
				entry.SetRouteNumberText(routeNumber);
			}
		}
	}
}
