using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using Requests;
using SimpleJSON;
using Dialog;

public class Splitwise : Widget
{
	[Header("Splitwise Settings")]
	[SerializeField] private Text groupName;
	[SerializeField] private Text people;
	[SerializeField] private Text amount;
	[SerializeField] private Text allSettledUp;

	private string groupId;
	private string apiKey;

	public override void ReloadConfig()
	{
		JSONNode config = Config.instance.GetWidgetConfig()[this.GetWidgetConfigKey()];
		groupId = config["groupId"];
		apiKey = config["apiKey"];
	}

    public override void Run()
	{
		this.ReloadConfig();
		StartCoroutine(RunRoutine());
		this.UpdateLastUpdatedText();
	}

	private IEnumerator RunRoutine()
	{
		UnityWebRequest request = Postman.CreateGetRequest(Endpoints.SPLITWISE(groupId, apiKey));
		yield return request.SendWebRequest();

		bool ok = request.error == null ? true : false;
		if (!ok)
		{
			WidgetLogger.instance.Log(this, "Error: " + request.error);
			yield break;
		}

		JSONNode json = JSON.Parse(request.downloadHandler.text);
		JSONNode exp = json["expenses"][0];

		groupName.text = json["groupName"];
		people.text 		= exp == null ? "" : exp["who"] + " owes " + exp["owes"];
		allSettledUp.text 	= exp == null ? "All settled up!" : "";
		amount.text = exp["amount"];
	}
}