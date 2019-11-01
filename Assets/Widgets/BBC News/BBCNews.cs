using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace BBCNews
{
	public class BBCNews : Widget
	{
		[Space(15f)]
		[SerializeField] private BBCNewsEntry entry;
		[SerializeField] private string apiKey;
		[SerializeField] private float secondsBetweenArticles = 20f;

		private BBCNewsJsonResponse response;
		private int currentArticleIndex = 0;

		private void Start()
		{
			this.Initialise();
			InvokeRepeating("Run", 0f, RepeatRateInSeconds());
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

			entry.GetTitle().text = responseItem.title;
			entry.GetDescription().text = responseItem.description;
			entry.SetUrl(responseItem.url);

			currentArticleIndex++;

			if (currentArticleIndex == response.articles.Length - 1)
			{
				currentArticleIndex = 0;
			}
		}
	}
}
