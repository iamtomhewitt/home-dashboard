using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System;
using System.Collections;
using SimpleJSON;
using Dialog;
using Requests;

namespace GoogleCalendar
{
	public class GoogleCalendar : FadingWidget
	{
		[Header("Google Calendar Settings")]
		[SerializeField] private GoogleCalendarEvent googleCalendarEventPrefab;
		[SerializeField] private Transform scrollParent;
		[SerializeField] private Image scrollbarBackground;
		[SerializeField] private Image scrollbarHandle;
		[SerializeField] private string gmailAddress;

		private JSONNode json;

		private string apiKey;
		private int numberOfEvents;

		public override void ReloadConfig()
		{
			JSONNode config = Config.instance.GetWidgetConfig()[this.GetWidgetConfigKey()];
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
			UnityWebRequest request = Postman.CreateGetRequest(Endpoints.GOOGLE_CALENDAR(apiKey, gmailAddress, numberOfEvents));
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
				DateTime time = DateTime.Parse(item["start"]);

				GoogleCalendarEvent eventEntry = Instantiate(googleCalendarEventPrefab, scrollParent).GetComponent<GoogleCalendarEvent>();
				eventEntry.SetNameText(item["summary"]);
				eventEntry.SetDateText(time.ToString("dd MMM"));
				eventEntry.SetDateTextColour(GetTextColour());
				eventEntry.SetNameTextColour(GetTextColour());
			}

			scrollbarBackground.color = Colours.Darken(GetWidgetColour());
			scrollbarHandle.color = Colours.Lighten(GetWidgetColour(), 0.1f);
		}
	}
}