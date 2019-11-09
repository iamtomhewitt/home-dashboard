using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using JsonHelper;
using JsonResponse;

namespace OnlineLists
{
    public class OnlineList : Widget
    {
        [Space(15f)]
        [SerializeField] private OnlineListEntry entryPrefab;
        [SerializeField] private Transform content;
        [SerializeField] private Text statusText;
		[SerializeField] private Config config;

		[SerializeField] private string configKeyName;

		private string publicKey;
		private string privateKey;
		private string dreamloUrl = "http://dreamlo.com/lb/";

		private void Start()
        {
			publicKey = config.GetConfig()["apiKeys"]["onlineLists"][configKeyName]["publicKey"];
			privateKey = config.GetConfig()["apiKeys"]["onlineLists"][configKeyName]["privateKey"];

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
            string url = dreamloUrl + publicKey + "/json";

            UnityWebRequest request = UnityWebRequest.Get(url);
            yield return request.SendWebRequest();
            string jsonResponse = OnlineListJsonHelper.StripParentFromJson(request.downloadHandler.text, 2);

            OnlineListData leaderboard = JsonUtility.FromJson<OnlineListData>(jsonResponse);

            if (leaderboard.entry != null)
            {
                // Remove previous entries so there are no duplicates
                foreach (Transform g in content)
                {
                    Destroy(g.gameObject);
                }

                for (int i = 0; i < leaderboard.entry.Length; i++)
                {
                    OnlineListEntry e = Instantiate(entryPrefab, content).GetComponent<OnlineListEntry>();
                    e.GetNameText().text = leaderboard.entry[i].name;
                    e.SetRemoveUrl(dreamloUrl + privateKey + "/delete/" + e.GetNameText().text);
                }
            }
            else
            {
                OnlineListSingleData singleEntry = JsonUtility.FromJson<OnlineListSingleData>(jsonResponse);

                // Remove previous entries so there are no duplicates
                foreach (Transform g in content)
                {
                    Destroy(g.gameObject);
                }

                for (int i = 0; i < 1; i++)
                {
                    OnlineListEntry e = Instantiate(entryPrefab, content).GetComponent<OnlineListEntry>();
                    e.GetNameText().text = leaderboard.entry[i].name;
                    e.SetRemoveUrl(dreamloUrl + privateKey + "/delete/" + e.GetNameText().text);
                }
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