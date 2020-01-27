using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using Requests;
using SimpleJSON;

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

		JSONNode json = JSON.Parse(request.downloadHandler.text);
		JSONNode exp = json["expenses"][0];

		groupName.text = json["groupName"];
		people.text = exp["who"] + " owes " + exp["owes"];
		amount.text = exp["amount"];
	}
}