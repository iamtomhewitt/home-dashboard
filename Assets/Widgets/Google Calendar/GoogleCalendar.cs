using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using SimpleJSON;

namespace GoogleCalendar
{
	public class GoogleCalendar : Widget
	{
		[Space(15f)]
		[SerializeField] private GoogleCalendarEvent googleCalendarEventPrefab;
		[SerializeField] private Transform scrollParent;
		[SerializeField] private Config config;

		[SerializeField] private string gmailAddress;
		[SerializeField] private string apiKeyConfigKeyName;

		private JSONNode json;

		private string apiKey;

		private void Start()
		{
			apiKey = config.GetConfig()["apiKeys"]["googleCalendars"][apiKeyConfigKeyName];

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
			string today = DateTime.Now.ToString("yyyy-MM-dd");
			string future = DateTime.Now.AddMonths(6).ToString("yyyy-MM-dd");

			string url = "https://www.googleapis.com/calendar/v3/calendars/" + gmailAddress +
							"/events?orderBy=startTime&singleEvents=true&timeMax=" + future + "T10:00:00-07:00&timeMin=" + today + "T10:00:00-07:00&key=" +
							apiKey;

			UnityWebRequest request = UnityWebRequest.Get(url);
			yield return request.SendWebRequest();
			string response = request.downloadHandler.text;

			json = JSON.Parse(response);

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
			}
		}
	}
}
