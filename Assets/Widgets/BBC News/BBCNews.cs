using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using SimpleJSON;

namespace BBCNews
{
	public class BBCNews : Widget
	{
		[Space(15f)]
		[SerializeField] private BBCNewsEntry entry;
		[SerializeField] private Config config;
		[SerializeField] private float secondsBetweenArticles = 20f;

		private JSONNode json;

		private string apiKey;
		private int currentArticleIndex = 0;

		private void Start()
		{
			this.Initialise();
			apiKey = config.GetConfig()["apiKeys"]["bbcNews"];

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
			string response = request.downloadHandler.text;

			json = JSON.Parse(response);
		}

		private void Cycle()
		{
			string title		= json["articles"][currentArticleIndex]["title"];
			string description	= json["articles"][currentArticleIndex]["description"];
			string url			= json["articles"][currentArticleIndex]["url"];

			entry.GetTitle().text = title;
			entry.GetDescription().text = description;
			entry.SetUrl(url);

			currentArticleIndex++;

			if (currentArticleIndex == json["articles"].Count - 1)
			{
				currentArticleIndex = 0;
			}
		}
	}
}