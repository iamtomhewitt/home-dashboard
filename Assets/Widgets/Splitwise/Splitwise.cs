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

	private string groupId;

	public override void ReloadConfig()
	{
		JSONNode config = Config.instance.GetWidgetConfig()[this.GetWidgetConfigKey()];
		groupId = config["groupId"];
	}

    public override void Run()
	{
		this.ReloadConfig();
		StartCoroutine(RunRoutine());
		this.UpdateLastUpdatedText();
	}

	private IEnumerator RunRoutine()
	{
		UnityWebRequest request = Postman.CreateGetRequest(Endpoints.SPLITWISE(groupId));
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
		people.text = exp["who"] + " owes " + exp["owes"];
		amount.text = exp["amount"];
	}
}