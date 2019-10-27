using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace OnlineLists
{
	public class OnlineListEntry : MonoBehaviour
	{
		public Text nameText;
		public string removeUrl;

		// Called from the 'X' button
		public void Remove()
		{
			StartCoroutine(RemoveRoutine());
		}

		private IEnumerator RemoveRoutine()
		{
			RemoveConfirmDialog dialog = FindObjectOfType<RemoveConfirmDialog>();
			dialog.Show("Remove '<b>" + nameText.text + "</b>'?");
			dialog.SetNone();

			while (dialog.GetResult() == RemoveConfirmDialog.DialogResult.NONE)
			{
				yield return null;
			}

			if (dialog.GetResult() == RemoveConfirmDialog.DialogResult.NO || dialog.GetResult() == RemoveConfirmDialog.DialogResult.CANCEL)
			{
				dialog.Hide();
				yield break;
			}

			if (dialog.GetResult() == RemoveConfirmDialog.DialogResult.YES)
			{
				dialog.Hide();

				UnityWebRequest request = UnityWebRequest.Get(removeUrl);
				yield return request.SendWebRequest();

				bool ok = request.downloadHandler.text.Equals("OK") ? true : false;
				if (!ok)
				{
					print(request.downloadHandler.text);
				}

				Destroy(this.gameObject);
			}
		}
	}
}