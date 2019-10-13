using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

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
