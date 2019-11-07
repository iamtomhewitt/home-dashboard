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
			[SerializeField] private string dreamloPrivateKey;

			private string dreamloUrl = "http://dreamlo.com/lb/";

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
				string url = dreamloUrl + dreamloPrivateKey + "/add/" + item + "/0";

				UnityWebRequest request = UnityWebRequest.Get(url);
				yield return request.SendWebRequest();

				bool ok = request.downloadHandler.text.Equals("OK") ? true : false;
				if (!ok)
				{
					statusText.text = "ERROR UPLOADING: " + request.downloadHandler.text;
				}

				list.Refresh();

				statusText.text = "'" + item + "' uploaded!";
			}
		}
	}
}
