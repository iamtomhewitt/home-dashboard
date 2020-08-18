using Dialog;
using Requests;
using SimpleJSON;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine;

namespace BBCNews
{
	public class BBCNews : FadingWidget
	{
		[Header("BBC News Settings")]
		[SerializeField] private BBCNewsEntry entry;
		[SerializeField] private Image logo;

		private Color logoColour;
		private JSONNode json;

		private float secondsBetweenArticles;
		private int currentArticleIndex = 0;

		public override void Start()
		{
			base.Start();
			InvokeRepeating("Cycle", 1f, secondsBetweenArticles);
		}

		public override void ReloadConfig()
		{
			JSONNode config = Config.instance.GetWidgetConfig()[this.GetWidgetConfigKey()];
			secondsBetweenArticles = config["secondsBetweenArticles"];
			logoColour = Colours.ToColour(config["logoColour"]);
		}

		public override void Run()
		{
			this.ReloadConfig();
			StartCoroutine(RequestHeadlines());

			this.SetWidgetColour(GetWidgetColour());
			logo.color = logoColour;
			entry.SetTextColour(GetTextColour());

			this.UpdateLastUpdatedText();
		}

		private IEnumerator RequestHeadlines()
		{
			UnityWebRequest request = UnityWebRequest.Get(Endpoints.instance.BBC_NEWS());
			yield return request.SendWebRequest();
			string response = request.downloadHandler.text;

			bool ok = request.error == null ? true : false;
			if (!ok)
			{
				WidgetLogger.instance.Log(this, "Error: " + request.error);
				yield break;
			}

			json = JSON.Parse(response);
		}

		private void Cycle()
		{
			StartCoroutine(Fade(SwitchArticle, 1f));
		}

		private void SwitchArticle()
		{
			if (json != null)
			{
				string title = json["articles"][currentArticleIndex]["title"];
				string description = json["articles"][currentArticleIndex]["description"];
				string url = json["articles"][currentArticleIndex]["url"];

				entry.SetTitle(title);
				entry.SetDescription(description);
				entry.SetUrl(url);

				currentArticleIndex = ++currentArticleIndex > json["articles"].Count - 1 ? 0 : currentArticleIndex;
			}
		}
	}
}