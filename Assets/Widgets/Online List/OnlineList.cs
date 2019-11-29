using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using SimpleJSON;
using Dialog;

namespace OnlineLists
{
    public class OnlineList : FadingWidget
    {
        [Space(15f)]
        [SerializeField] private OnlineListEntry entryPrefab;
        [SerializeField] private Transform content;
        [SerializeField] private Text statusText;
		[SerializeField] private TodoistList listType;

		private string apiKey;

		private void Start()
        {
			apiKey = Config.instance.GetConfig()["apiKeys"]["todoist"];

            this.Initialise();
            InvokeRepeating("Run", 0f, RepeatRateInSeconds());
        }

        public override void Run()
        {
            StartCoroutine(Fade(RunRoutine, 1f));
            this.UpdateLastUpdatedText();
        }

        private IEnumerator RunRoutine()
        {
			string projectId = Config.instance.GetConfig()["todoist"][listType.ToString()];
			string url = "https://api.todoist.com/rest/v1/tasks?project_id=" + projectId;

			UnityWebRequest request = UnityWebRequest.Get(url);
			request.SetRequestHeader("Authorization", "Bearer " + apiKey);
			yield return request.SendWebRequest();
			string response = request.downloadHandler.text;

			bool ok = request.error == null ? true : false;
			if (!ok)
			{
				WidgetLogger.instance.Log(this, "Error: " + request.error);
			}

			// Remove previous entries so there are no duplicates
			foreach (Transform g in content)
			{
				Destroy(g.gameObject);
			}

			JSONNode json = JSON.Parse(response);
			foreach (JSONNode task in json)
			{
				OnlineListEntry e = Instantiate(entryPrefab, content).GetComponent<OnlineListEntry>();
				e.GetNameText().text = task["content"].Value;
				e.SetTaskId(task["id"].Value);
			}
		}

		/// <summary>
		/// Refreshes the list by re-running the Run() routine.
		/// </summary>
		public void Refresh()
		{
			Run();
		}
    }

	public enum TodoistList { TODO, Shopping};
}