using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using SimpleJSON;
using Dialog;
using System.Collections.Generic;
using Requests;

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
		private string projectId;

		private void Start()
		{
			this.ReloadConfig();
			this.Initialise();
			InvokeRepeating("Run", 0f, RepeatRateInSeconds());
			InvokeRepeating("UploadMissingItems", 30f, 10f);
		}

		public override void ReloadConfig()
		{
			JSONNode config = Config.instance.GetConfig()[this.GetWidgetConfigKey()];
			apiKey = config["apiKey"];
			projectId = config["todoistId"];
		}

		public override void Run()
		{
			this.ReloadConfig();
			StartCoroutine(Fade(RunRoutine, 1f));
			this.UpdateLastUpdatedText();
		}

		private void UploadMissingItems()
		{
			foreach (string item in itemsNotUploaded)
			{
				WidgetLogger.instance.Log("<b>" + listType.ToString() + " List </b>: Attempting to reupload " + item);
				AddItem(item);
			}
		}

		private IEnumerator RunRoutine()
		{
			UnityWebRequest request = Postman.CreateGetRequest(Endpoints.TODOIST_PROJECT(projectId));
			request.SetRequestHeader("Authorization", "Bearer " + apiKey);
			yield return request.SendWebRequest();

			bool ok = request.error == null ? true : false;
			if (!ok)
			{
				WidgetLogger.instance.Log(this, "Error: " + request.error + "\n URL: " + Endpoints.TODOIST_PROJECT(projectId));
				yield break;
			}

			// Remove previous entries so there are no duplicates
			foreach (Transform g in content)
			{
				Destroy(g.gameObject);
			}

			JSONNode json = JSON.Parse(request.downloadHandler.text);
			foreach (JSONNode task in json)
			{
				OnlineListEntry e = Instantiate(entryPrefab, content).GetComponent<OnlineListEntry>();
				e.SetNameText(task["content"].Value);
				e.SetTaskId(task["id"].Value);
				e.SetApiKey(apiKey);
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
			string uuid = System.Guid.NewGuid().ToString();
			string json = "{\"content\": \"" + item + "\", \"project_id\": " + projectId + " }";

			UnityWebRequest request = Postman.CreateTodoistRequest(Endpoints.TODOIST_TASKS, json, apiKey, uuid);
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

	public enum TodoistList { todoList, shoppingList };
}