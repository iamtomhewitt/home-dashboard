using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class BBCNews : Widget
{
	[Space(15f)]
	public BBCNewsEntry entry;
	public string apiKey;
	public float secondsBetweenArticles = 20f;

	private BBCNewsJsonResponse response;
	private int currentArticleIndex = 0;

	private void Start()
	{
		this.Initialise();
		InvokeRepeating("Run", 0f, ToSeconds(this.timeUnit, this.repeatRate));
		InvokeRepeating("Cycle", 1f, secondsBetweenArticles);
	}

	public override void Run()
	{
		StartCoroutine(RequestHeadlines());
		this.UpdateLastUpdatedText();
	}

	private IEnumerator RequestHeadlines()
	{
		string url = "https://newsapi.org/v2/top-headlines?sources=bbc-news&apiKey=" + apiKey;

		UnityWebRequest request = UnityWebRequest.Get(url);
		yield return request.SendWebRequest();
		string jsonResponse = request.downloadHandler.text;

		response = JsonUtility.FromJson<BBCNewsJsonResponse>(jsonResponse);
	}

	private void Cycle()
	{
		Article responseItem = response.articles[currentArticleIndex];

		entry.title.text = responseItem.title;
		entry.description.text = responseItem.description;
		entry.url = responseItem.url;

		currentArticleIndex++;

		if (currentArticleIndex == response.articles.Length - 1)
		{
			currentArticleIndex = 0; 
		}
	}
}
