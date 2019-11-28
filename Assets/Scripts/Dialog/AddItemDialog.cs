using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using OnlineLists;

namespace Dialog
{
	namespace OnlineLists
	{
		public class AddItemDialog : Dialog
		{
			[SerializeField] private OnlineList list;
			[SerializeField] private Text statusText;
			[SerializeField] private string todoistProjectKeyName;

			/// <summary>
			/// Called from a button.
			/// </summary>
			public void ResetStatusText()
			{
				statusText.text = "";
			}

			/// <summary>
			/// Called from a button.
			/// </summary>
			public void AddItem(InputField input)
			{
				StartCoroutine(AddItemRoutine(input.text));
				input.text = "";
			}

			public void AddItem(string item)
			{
				StartCoroutine(AddItemRoutine(item));
			}

			private IEnumerator AddItemRoutine(string item)
			{
				Config config = FindObjectOfType<Config>();

				string url			= "https://api.todoist.com/rest/v1/tasks";
				string apiKey		= config.GetConfig()["apiKeys"]["todoist"];
				string projectId	= config.GetConfig()["todoist"][todoistProjectKeyName];
				string uuid			= System.Guid.NewGuid().ToString();
				string json			= "{\"content\": \"" + item + "\", \"project_id\": " + projectId + " }";

				UnityWebRequest request = CreateRequest(url, json, apiKey, uuid);
				yield return request.SendWebRequest();
				
				bool ok = request.error == null ? true : false;
				if (!ok)
				{
					WidgetLogger.instance.Log("<b>Add Dialog</b>: Could not add item: " + request.downloadHandler.text);
				}

				list.Refresh();

				statusText.text = "'" + item + "' uploaded!";
			}

			/// <summary>
			/// Creates a UnityWebRequest for Todoist
			/// </summary>
			private UnityWebRequest CreateRequest(string url, string json, string apiKey, string uuid)
			{
				UnityWebRequest request = new UnityWebRequest(url, "POST");

				byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);

				request.uploadHandler = new UploadHandlerRaw(jsonToSend);
				request.downloadHandler = new DownloadHandlerBuffer();

				request.SetRequestHeader("Content-Type", "application/json");
				request.SetRequestHeader("Authorization", "Bearer " + apiKey);
				request.SetRequestHeader("X-Request-Id", uuid);

				return request;
			}
		}
	}
}
