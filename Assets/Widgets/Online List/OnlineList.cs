using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Dialog;
using Requests;
using SimpleJSON;
using TMPro;

namespace OnlineLists
{
	public class OnlineList : FadingWidget
	{
		[Header("Online List Settings")]
		[SerializeField] private TodoistList listType;
		[SerializeField] private OnlineListEntry entryPrefab;
		[SerializeField] private Transform content;
		[SerializeField] private TMP_Text addButtonText;
		[SerializeField] private Image addButtonColour;
		[SerializeField] private Image scrollbarBackground;
		[SerializeField] private Image scrollbarHandle;

		private List<string> itemsNotUploaded = new List<string>();
		private AddItemDialog addDialog;
		private string apiKey;
		private string projectId;
		private float missingItemsUploadRate = 30f;

		public override void Start()
		{
			base.Start();
			InvokeRepeating("UploadMissingItems", missingItemsUploadRate, missingItemsUploadRate);
		}

		private void UploadMissingItems()
		{
			foreach (string item in itemsNotUploaded)
			{
				WidgetLogger.instance.Log("<b>" + listType.ToString() + " List </b>: Attempting to reupload " + item);
				AddItem(item);
			}
		}

		public override void ReloadConfig()
		{
			JSONNode config = Config.instance.GetWidgetConfig()[this.GetWidgetConfigKey()];
			apiKey = config["apiKey"];
			projectId = config["todoistId"];
		}

		public override void Run()
		{
			this.ReloadConfig();
			StartCoroutine(Fade(RunRoutine, 1f));
			this.UpdateLastUpdatedText();
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
				e.SetNameTextColour(GetTextColour());
				e.SetRemoveButtonTextColour(GetTextColour());
				e.SetTaskId(task["id"].Value);
				e.SetApiKey(apiKey);
			}

			addButtonColour.color = Colours.Darken(GetWidgetColour());
			addButtonText.color = GetTextColour();
			scrollbarBackground.color = Colours.Darken(GetWidgetColour());
			scrollbarHandle.color = Colours.Lighten(GetWidgetColour());
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
			string body = JsonBody.TodoistTask(item, projectId);

			UnityWebRequest request = Postman.CreatePostRequest(Endpoints.TODOIST_TASKS, body);
			request.SetRequestHeader("Authorization", "Bearer " + apiKey);
			request.SetRequestHeader("X-Request-Id", uuid);
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

			if (addDialog != null)
			{
				addDialog.SetStatusText(string.Format("'{0}' uploaded!", item));
			}
		}

		/// <summary>
		/// Refreshes the list by re-running the Run() routine.
		/// </summary>
		public void Refresh()
		{
			Run();
		}

		/// <summary>
		/// Called from the Add button.
		/// </summary>
		public void ShowAddDialog()
		{
			addDialog = FindObjectOfType<AddItemDialog>();
			addDialog.SetParentWidget(this);
			addDialog.SetOnlineList(this);
			addDialog.ResetStatusText();
			addDialog.ApplyColours();
			addDialog.Show();
		}

		public TodoistList GetListType()
		{
			return listType;
		}
	}

	public enum TodoistList { todoList, shoppingList };
}