using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Requests;
using SimpleJSON;

public class Splitwise : Widget
{
	[Header("Splitwise Settings")]
	[SerializeField] private Text groupName;
	[SerializeField] private Text people;
	[SerializeField] private Text amount;

	private string groupId;
	
	private void Start()
    {
        this.Initialise();
		groupId = Config.instance.GetConfig()["splitwise"]["groupId"];
		InvokeRepeating("Run", 0f, RepeatRateInSeconds());
    }

    public override void Run()
	{
		groupId = Config.instance.GetConfig()["splitwise"]["groupId"];
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
