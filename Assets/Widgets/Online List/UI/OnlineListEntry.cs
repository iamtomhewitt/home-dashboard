using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using Dialog;
using Requests;
using SimpleJSON;

namespace OnlineLists
{
	public class OnlineListEntry : MonoBehaviour
	{
		[SerializeField] private Text nameText;
		[SerializeField] private string taskId;

		private string apiKey;

		public void SetNameText(string text)
		{
			nameText.text = text;
		}

		public Text GetNameText()
		{
			return nameText;
		}

		public void SetTaskId(string id)
		{
			taskId = id;
		}

		public void SetApiKey(string apiKey)
		{
			this.apiKey = apiKey;
		}

		// Called from the 'X' button
		public void Remove()
		{
			StartCoroutine(RemoveRoutine());
		}

		private IEnumerator RemoveRoutine()
		{
			ConfirmDialog dialog = FindObjectOfType<ConfirmDialog>();
			dialog.Show();
			dialog.SetInfoMessage("Remove '<b>" + nameText.text + "</b>'?");
			dialog.None();

			while (dialog.GetResult() == DialogResult.NONE)
			{
				yield return null;
			}

			if (dialog.GetResult() == DialogResult.NO || dialog.GetResult() == DialogResult.CANCEL)
			{
				dialog.Hide();
				yield break;
			}

			if (dialog.GetResult() == DialogResult.YES)
			{
				dialog.Hide();

				string url = "https://api.todoist.com/rest/v1/tasks/" + taskId + "/close";

				UnityWebRequest request = Postman.CreatePostRequest(Endpoints.TODOIST_TASKS + "/" + taskId + "/close", new JSONObject());
				request.SetRequestHeader("Authorization", "Bearer " + apiKey);
				yield return request.SendWebRequest();

				Destroy(this.gameObject);
			}
		}
	}
}