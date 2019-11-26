using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using SimpleJSON;

namespace OnlineLists
{
    public class OnlineList : Widget
    {
        [Space(15f)]
        [SerializeField] private OnlineListEntry entryPrefab;
        [SerializeField] private Transform content;
        [SerializeField] private Text statusText;
		[SerializeField] private Config config;

		[SerializeField] private string todoistProjectKeyName;

		private string apiKey;

		private void Start()
        {
			apiKey = config.GetConfig()["apiKeys"]["todoist"];

            this.Initialise();
            InvokeRepeating("Run", 0f, RepeatRateInSeconds());
        }

        public override void Run()
        {
            StartCoroutine(RunRoutine());
            this.UpdateLastUpdatedText();
        }

        private IEnumerator RunRoutine()
        {
			string projectId = config.GetConfig()["todoist"][todoistProjectKeyName];
			string url = "https://api.todoist.com/rest/v1/tasks?project_id=" + projectId;

			UnityWebRequest request = UnityWebRequest.Get(url);
			request.SetRequestHeader("Authorization", "Bearer " + apiKey);
			yield return request.SendWebRequest();
			string response = request.downloadHandler.text;

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
}