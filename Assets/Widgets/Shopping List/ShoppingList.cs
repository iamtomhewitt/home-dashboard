using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class ShoppingList : Widget
{
	[Space(15f)]
	public string publicKey;
	public string privateKey;
	private string dreamloUrl = "http://dreamlo.com/lb/";

	public ShoppingListEntry entryPrefab;
	public Transform content;

	private void Start()
	{
		this.Initialise();
		InvokeRepeating("Run", 0f, ToSeconds());
	}

	public override void Run()
	{
		StartCoroutine(RunRoutine());
		this.UpdateLastUpdatedText();
	}

	private IEnumerator RunRoutine()
	{
		string url = dreamloUrl + publicKey + "/json";

		UnityWebRequest request = UnityWebRequest.Get(url);
		yield return request.SendWebRequest();
		string jsonResponse = ShoppingListJsonHelper.StripParentFromJson(request.downloadHandler.text, 2);
		ShoppingListData leaderboard = JsonUtility.FromJson<ShoppingListData>(jsonResponse);

		// Remove previous entries so there are no duplicates
		foreach (Transform g in content)
		{
			Destroy(g.gameObject);
		}

		for (int i = 0; i < leaderboard.entry.Length; i++)
		{
			ShoppingListEntry e = Instantiate(entryPrefab, content).GetComponent<ShoppingListEntry>();
			e.nameText.text = leaderboard.entry[i].name;
		}
	}

	public void AddItem()
	{

	}
}
