using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using SimpleJSON;
using Dialog;
using Requests;

namespace Train
{
    public class Trains : FadingWidget
    {
        [Header("Train Settings")]
        [SerializeField] private TrainEntry[] trainEntries;
        [SerializeField] private Image scrollbarBackground;
        [SerializeField] private Image scrollbarHandle;

        private JSONNode config;

        private string apiToken;
        private string stationCode;

        private int maxDestinationLength = 10;
		private int lastRowNumber = 0;

        public override void ReloadConfig()
        {
            config = Config.instance.GetWidgetConfig()[this.GetWidgetConfigKey()];
            apiToken = config["apiKey"];
            stationCode = config["stationCode"];
        }

        public override void Run()
        {
            this.ReloadConfig();

            StartCoroutine(Fade(RunRoutine, 1f));
            scrollbarBackground.color = Colours.Darken(GetWidgetColour());
            scrollbarHandle.color = Colours.Lighten(GetWidgetColour());

            this.UpdateLastUpdatedText();
        }

        private IEnumerator RunRoutine()
        {
            UnityWebRequest request = Postman.CreateGetRequest(Endpoints.TRAIN_DEPARTURES("SAL", trainEntries.Length, apiToken));
            yield return request.SendWebRequest();

            JSONNode json = JSON.Parse(request.downloadHandler.text);

            bool ok = request.error == null ? true : false;
            if (!ok)
            {
                WidgetLogger.instance.Log(this, "Error: " + request.error);
                yield break;
            }

            foreach (TrainEntry entry in trainEntries)
            {
                entry.SetDestinationText("");
                entry.SetTimeText("");
            }

            lastRowNumber = 0;

			yield return PopulateEntries(json, "trainServices");
			yield return PopulateEntries(json, "busServices");
        }

        private IEnumerator PopulateEntries(JSONNode json, string key)
        {
            for (int i = 0; i < json[key].Count; i++)
            {
                if (json[key].Count == i)
                {
                    yield break;
                }

                TrainEntry entry = trainEntries[lastRowNumber];
                JSONNode trainService = json[key][i];
                string locationName = trainService["destination"][0]["locationName"];
				string time = key.Equals("busServices") ? trainService["std"] + " (Bus)" : trainService["std"] + " (" + trainService["etd"] + ")";

                if (locationName.Length > maxDestinationLength)
                {
                    locationName = locationName.Substring(0, maxDestinationLength - 1) + "...";
                }
                
				entry.SetDestinationText(locationName);
                entry.SetTimeText(time);

                lastRowNumber++;
            }
        }
    }
}
