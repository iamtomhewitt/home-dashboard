using Dialog;
using Requests;
using SimpleJSON;
using System.Collections;
using System;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine;

namespace GoogleCalendar
{
	public class GoogleCalendar : FadingWidget
	{
		[Header("Google Calendar Settings")]
		[SerializeField] private GoogleCalendarEvent googleCalendarEventPrefab;
		[SerializeField] private Image scrollbarBackground;
		[SerializeField] private Image scrollbarHandle;
		[SerializeField] private Transform scrollParent;
		[SerializeField] private string gmailAddress;

		private JSONNode json;
		private int numberOfEvents;
		private string apiKey;

		public override void ReloadConfig()
		{
			JSONNode config = this.GetConfig();
			apiKey = config["apiKey"];
			numberOfEvents = config["numberOfEvents"];
		}

		public override void Run()
		{
			this.ReloadConfig();
			StartCoroutine(Fade(RunRoutine, 1f));
			this.UpdateLastUpdatedText();
		}

		private IEnumerator RunRoutine()
		{
			UnityWebRequest request = Postman.CreateGetRequest(Endpoints.instance.GOOGLE_CALENDAR(gmailAddress, numberOfEvents, apiKey));
			yield return request.SendWebRequest();

			json = JSON.Parse(request.downloadHandler.text);

			bool ok = request.error == null ? true : false;
			if (!ok)
			{
				WidgetLogger.instance.Log(this, "Error: " + request.error);
				yield break;
			}

			// Remove old events
			foreach (Transform child in scrollParent)
			{
				Destroy(child.gameObject);
			}

			for (int i = 0; i < json["events"].Count; i++)
			{
				JSONNode item = json["events"][i];
				DateTime startDate = DateTime.Parse(item["start"]);
				DateTime endDate = DateTime.Parse(item["end"]);

				GoogleCalendarEvent eventEntry = Instantiate(googleCalendarEventPrefab, scrollParent).GetComponent<GoogleCalendarEvent>();
				eventEntry.SetDescription(item["description"]);
				eventEntry.SetEndDateText(endDate.ToString("dd MMM"));
				eventEntry.SetEndTime(item["endTime"]);
				eventEntry.SetGoogleCalendar(this);
				eventEntry.SetLocation(item["location"]);
				eventEntry.SetStartDateText(startDate.ToString("dd MMM"));
				eventEntry.SetStartDateTextColour(GetTextColour());
				eventEntry.SetStartTime(item["startTime"]);
				eventEntry.SetSummaryText(item["summary"]);
				eventEntry.SetSummaryTextColour(GetTextColour());
			}

			scrollbarBackground.color = Colours.Darken(GetWidgetColour());
			scrollbarHandle.color = Colours.Lighten(GetWidgetColour(), 0.1f);
		}
	}
}