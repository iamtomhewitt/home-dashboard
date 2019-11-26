using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using Dialog;

namespace OnlineLists
{
	public class OnlineListEntry : MonoBehaviour
	{
		[SerializeField] private Text nameText;
		[SerializeField] private string taskId;

		public Text GetNameText()
		{
			return nameText;
		}

		public void SetTaskId(string id)
		{
			taskId = id;
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

				Config config = FindObjectOfType<Config>();

				string url = "https://api.todoist.com/rest/v1/tasks/" + taskId + "/close";
				string apiKey = config.GetConfig()["apiKeys"]["todoist"];

				UnityWebRequest request = UnityWebRequest.Post(url, "");
				request.SetRequestHeader("Authorization", "Bearer " + apiKey);
				yield return request.SendWebRequest();

				Destroy(this.gameObject);
			}
		}
	}
}