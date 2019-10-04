using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class BBCNews : Widget
{
	[Space(15f)]
	public BBCNewsEntry entry;

	public string apiKey;

	public int secondsBetweenArticles = 20;

	private void Start()
	{
		this.Initialise();
		InvokeRepeating("Run", 0f, this.repeatRate);
	}
	
	public override void Run()
	{
		StartCoroutine(RunRoutine());
		this.UpdateLastUpdatedText();
	}

	private IEnumerator RunRoutine()
	{
		string url = "https://newsapi.org/v2/top-headlines?sources=bbc-news&apiKey=" + apiKey;

		UnityWebRequest request = UnityWebRequest.Get(url);
		yield return request.SendWebRequest();
		string jsonResponse = request.downloadHandler.text;

		BBCNewsJsonResponse response = JsonUtility.FromJson<BBCNewsJsonResponse>(jsonResponse);

		for (int i = 0; i < response.articles.Length; i++)
		{
			Article responseItem = response.articles[i];

			entry.title.text = responseItem.title;
			entry.description.text = responseItem.description;

			yield return new WaitForSeconds(secondsBetweenArticles);

			if (i == response.articles.Length - 1)
			{
				i = -1; // will be set to 0 on next loop
			}
		}
	}
}
