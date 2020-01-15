using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using SimpleJSON;
using Dialog;
using Requests;

namespace BBCNews
{
	public class BBCNews : FadingWidget
	{
		[Header("BBC News Settings")]
		[SerializeField] private BBCNewsEntry entry;
		[SerializeField] private float secondsBetweenArticles = 60f;

		private JSONNode json;

		private string apiKey;
		private int currentArticleIndex = 0;

		private void Start()
		{
			this.Initialise();
			apiKey = Config.instance.GetConfig()["apiKeys"]["bbcNews"];

			InvokeRepeating("Run", 0f, RepeatRateInSeconds());
			InvokeRepeating("Cycle", 1f, secondsBetweenArticles);			
		}

		public override void Run()
		{
			apiKey = Config.instance.GetConfig()["apiKeys"]["bbcNews"];
			StartCoroutine(RequestHeadlines());
			this.UpdateLastUpdatedText();
		}

		private IEnumerator RequestHeadlines()
		{
			UnityWebRequest request = UnityWebRequest.Get(Endpoints.BBC_NEWS(apiKey));
			yield return request.SendWebRequest();
			string response = request.downloadHandler.text;

			bool ok = request.error == null ? true : false;
			if (!ok)
			{
				WidgetLogger.instance.Log(this, "Error: " + request.error);
			}

			json = JSON.Parse(response);
		}

		private void Cycle()
		{
			StartCoroutine(Fade(SwitchArticle, 1f));
		}

		private void SwitchArticle()
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