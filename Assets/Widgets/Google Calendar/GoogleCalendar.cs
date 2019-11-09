using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using JsonResponse;

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
			string jsonResponse = request.downloadHandler.text;

			GoogleCalendarJsonResponse response = JsonUtility.FromJson<GoogleCalendarJsonResponse>(jsonResponse);

			// Remove old events
			foreach (Transform child in scrollParent)
			{
				Destroy(child.gameObject);
			}

			for (int i = 0; i < response.items.Length; i++)
			{
				if (response.items.Length == i)
				{
					yield break;
				}

				Item responseItem = response.items[i];

				// There could be two different types of date to use, so figure out which one to use and substring accordingly
				string dateToUse = responseItem.start.date == null ? responseItem.start.dateTime.Substring(0, 10) : responseItem.start.date;

				// Convert to a date time
				DateTime time = DateTime.Parse(dateToUse);

				// And populate
				GoogleCalendarEvent eventEntry = Instantiate(googleCalendarEventPrefab, scrollParent).GetComponent<GoogleCalendarEvent>();
				eventEntry.GetNameText().text = responseItem.summary;
				eventEntry.GetDateText().text = time.ToString("dd MMM");
			}
		}
	}
}
