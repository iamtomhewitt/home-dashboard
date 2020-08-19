using Dialog;
using Requests;
using SimpleJSON;
using System.Collections;
using TMPro;
using UnityEngine.Networking;
using UnityEngine;

public class Splitwise : Widget
{
	[Header("Splitwise Settings")]
	[SerializeField] private TMP_Text allSettledUp;
	[SerializeField] private TMP_Text amount;
	[SerializeField] private TMP_Text groupName;
	[SerializeField] private TMP_Text people;

	private string apiKey;
	private string groupId;

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
		UnityWebRequest request = Postman.CreateGetRequest(Endpoints.instance.SPLITWISE());
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
		people.text = exp == null ? "" : exp["who"] + " owes " + exp["owes"];
		allSettledUp.text = exp == null ? "All settled up!" : "";
		amount.text = exp["amount"];
	}
}