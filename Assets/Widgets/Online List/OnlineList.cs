using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using SimpleJSON;
using Dialog;
using System.Collections.Generic;

namespace OnlineLists
{
	public class OnlineList : FadingWidget
	{
		[Header("Online List Settings")]
		[SerializeField] private OnlineListEntry entryPrefab;
		[SerializeField] private Transform content;
		[SerializeField] private Text statusText;
		[SerializeField] private TodoistList listType;

		private List<string> itemsNotUploaded = new List<string>();
		private string apiKey;

		private void Start()
		{
			apiKey = Config.instance.GetConfig()["apiKeys"]["todoist"];

			this.Initialise();
			InvokeRepeating("Run", 0f, RepeatRateInSeconds());
			InvokeRepeating("UploadMissingItems", 30f, 10f);
		}

		private void UploadMissingItems()
		{
			foreach (string item in itemsNotUploaded)
			{
				WidgetLogger.instance.Log("<b>" + listType.ToString() + " List </b>: Attempting to reupload " + item);
				AddItem(item);
			}
		}

		public override void Run()
		{
			StartCoroutine(Fade(RunRoutine, 1f));
			this.UpdateLastUpdatedText();
		}

		private IEnumerator RunRoutine()
		{
			string projectId = Config.instance.GetConfig()["todoist"][listType.ToString()];
			string url = "https://api.todoist.com/rest/v1/tasks?project_id=" + projectId;

			UnityWebRequest request = UnityWebRequest.Get(url);
			request.SetRequestHeader("Authorization", "Bearer " + apiKey);
			yield return request.SendWebRequest();
			string response = request.downloadHandler.text;

			bool ok = request.error == null ? true : false;
			if (!ok)
			{
				WidgetLogger.instance.Log(this, "Error: " + request.error + "\n URL: " + url);
				yield break;
			}

			// Remove previous entries so there are no duplicates
			foreach (Transform g in content)
			{
				Destroy(g.gameObject);
			}

			JSONNode json = JSON.Parse(response);
			foreach (JSONNode task in json)
			{
				OnlineListEntry e = Instantiate(entryPrefab, content).GetComponent<OnlineListEntry>();
				e.GetNameText().text = task["content"].Value;
				e.SetTaskId(task["id"].Value);
			}
		}

		/// <summary>
		/// Adds an item to the relevant list on Todoist.
		/// </summary>
		public void AddItem(string item)
		{
			StartCoroutine(AddItemRoutine(item));
		}

		private IEnumerator AddItemRoutine(string item)
		{
			Config config = FindObjectOfType<Config>();

			string url = "https://api.todoist.com/rest/v1/tasks";
			string apiKey = config.GetConfig()["apiKeys"]["todoist"];
			string projectId = config.GetConfig()["todoist"][listType.ToString()];
			string uuid = System.Guid.NewGuid().ToString();
			string json = "{\"content\": \"" + item + "\", \"project_id\": " + projectId + " }";

			UnityWebRequest request = Postman.CreateTodoistRequest(url, json, apiKey, uuid);
			yield return request.SendWebRequest();

			bool ok = request.error == null ? true : false;
			if (!ok)
			{
				WidgetLogger.instance.Log("<b>" + listType.ToString() + " List</b>: Could not add item (" + item + "): " + request.downloadHandler.text);
				itemsNotUploaded.Add(item);
			}
			else if (itemsNotUploaded.Contains(item))
			{
				itemsNotUploaded.Remove(item);
			}

			Refresh();

			statusText.text = "'" + item + "' uploaded!";
		}

		/// <summary>
		/// Refreshes the list by re-running the Run() routine.
		/// </summary>
		public void Refresh()
		{
			Run();
		}
	}

	public enum TodoistList { TODO, Shopping };
}