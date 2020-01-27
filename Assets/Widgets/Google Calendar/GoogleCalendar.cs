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

		private void Start()
		{
			this.ReloadConfig();
			this.Initialise();
			InvokeRepeating("Run", 0f, RepeatRateInSeconds());
		}

		public override void ReloadConfig()
		{
			JSONNode config = Config.instance.GetWidgetConfig()[this.GetWidgetConfigKey()];
			apiKey = config["apiKey"];
		}

		public override void Run()
		{
			this.ReloadConfig();
			StartCoroutine(Fade(RunRoutine, 1f));
			this.UpdateLastUpdatedText();
		}

		private IEnumerator RunRoutine()
		{
			string today = DateTime.Now.ToString("yyyy-MM-dd");
			string future = DateTime.Now.AddMonths(6).ToString("yyyy-MM-dd");
			
			UnityWebRequest request = Postman.CreateGetRequest(Endpoints.GOOGLE_CALENDAR(gmailAddress, future, today, apiKey));
			yield return request.SendWebRequest();
			string response = request.downloadHandler.text;

			json = JSON.Parse(response);

			bool ok = request.error == null ? true : false;
			if (!ok)
			{
				WidgetLogger.instance.Log(this, "Error: " + request.error);
			}

			// Remove old events
			foreach (Transform child in scrollParent)
			{
				Destroy(child.gameObject);
			}

			for (int i = 0; i < json["items"].Count; i++)
			{
				if (json["items"].Count == i)
				{
					yield break;
				}

				JSONNode item = json["items"][i];

				// There could be two different types of date to use, so figure out which one to use and substring accordingly
				string dateToUse = item["start"]["date"] == null ? item["start"]["dateTime"].Value.Substring(0, 10) : item["start"]["date"].Value;

				// Convert to a date time
				DateTime time = DateTime.Parse(dateToUse);

				// And populate
				GoogleCalendarEvent eventEntry = Instantiate(googleCalendarEventPrefab, scrollParent).GetComponent<GoogleCalendarEvent>();
				eventEntry.GetNameText().text = item["summary"];
				eventEntry.GetDateText().text = time.ToString("dd MMM");
				eventEntry.SetDateTextColour(GetTextColour());
				eventEntry.SetNameTextColour(GetTextColour());
			}

			scrollbarBackground.color = Colours.Darken(GetWidgetColour());
			scrollbarHandle.color = Colours.Lighten(GetWidgetColour(), 0.1f);
		}
	}
}
