using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace OnlineLists
{
	public class OnlineListEntry : MonoBehaviour
	{
		[SerializeField] private Text nameText;
		[SerializeField] private string removeUrl;

		public Text GetNameText()
		{
			return nameText;
		}

		public void SetRemoveUrl(string url)
		{
			removeUrl = url;
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
			dialog.SetNone();

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