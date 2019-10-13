using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class OnlineList : Widget
{
	[Space(15f)]
	public string publicKey;
	public string privateKey;
	private string dreamloUrl = "http://dreamlo.com/lb/";

	[Space()]
	public OnlineListEntry entryPrefab;
	public Transform content;
	public GameObject addItemMenu;
	public Text statusText;

	private void Start()
	{
		this.Initialise();
		this.HideMenu();
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
		string jsonResponse = OnlineListJsonHelper.StripParentFromJson(request.downloadHandler.text, 2);
		OnlineListData leaderboard = JsonUtility.FromJson<OnlineListData>(jsonResponse);

		// Remove previous entries so there are no duplicates
		foreach (Transform g in content)
		{
			Destroy(g.gameObject);
		}

		for (int i = 0; i < leaderboard.entry.Length; i++)
		{
			OnlineListEntry e = Instantiate(entryPrefab, content).GetComponent<OnlineListEntry>();
			e.nameText.text = leaderboard.entry[i].name;
			e.removeUrl = dreamloUrl + privateKey + "/delete/" + e.nameText.text;
		}
	}

	private IEnumerator AddItem(string item)
	{
		string url = dreamloUrl + privateKey + "/add/" + item + "/0";

		UnityWebRequest request = UnityWebRequest.Get(url);
		yield return request.SendWebRequest();

		bool ok = request.downloadHandler.text.Equals("OK") ? true : false;
		if (!ok)
		{
			print(request.downloadHandler.text);
		}

		statusText.text = "'" + item + "' uploaded!";

		this.Run();
	}

	// Called from the pop up menu
	public void AddItem(InputField input)
	{
		StartCoroutine(AddItem(input.text));
		input.text = "";
	}

	public void RemoveItem()
	{

	}

	// Called from a button
	public void ShowMenu()
	{
		addItemMenu.SetActive(true);
		statusText.text = "";
	}

	// Called from a button
	public void HideMenu()
	{
		addItemMenu.SetActive(false);
		statusText.text = "";
	}	
}
