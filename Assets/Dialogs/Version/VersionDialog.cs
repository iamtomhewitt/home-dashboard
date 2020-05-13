using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using Dialog;
using TMPro;
using Requests;
using SimpleJSON;

public class VersionDialog : PopupDialog
{
	[SerializeField] private TMP_Text repoVersionText;
	[SerializeField] private TMP_Text installedVersionText;
	[SerializeField] private TMP_Text infoText;
	[SerializeField] private Button whatsNewButton;
	[SerializeField] private Color correctVersionColour;
	[SerializeField] private Color incorrectVersionColour;

	private VersionButton versionButton;
	private JSONNode repoInfo;
	private string repoVersion;
	private string installedVersion;
	private float TWELVE_HOURS = 43200f;
	private float ONE_WEEK = 604800f;

    private void Start()
    {
		InvokeRepeating("GetVersionInfo", 0f, TWELVE_HOURS);
		InvokeRepeating("ShowDialogIfVersionWrong", 3f, ONE_WEEK);
    }

	private void GetVersionInfo()
	{
		StartCoroutine(GetVersionInfoRoutine());
	}

	private IEnumerator GetVersionInfoRoutine()
	{
		UnityWebRequest request = Postman.CreateGetRequest(Config.instance.GetEndpoint("release"));
		yield return request.SendWebRequest();

		repoInfo = JSON.Parse(request.downloadHandler.text);
		repoVersion = repoInfo["name"];
		installedVersion = Application.version;

        FindObjectOfType<VersionButton>().SetVersionText("Version: " + installedVersion);

		installedVersionText.text = installedVersion;
		installedVersionText.color = string.Equals(installedVersion, repoVersion) ? correctVersionColour : incorrectVersionColour;
		repoVersionText.text = repoVersion;
		infoText.text = string.Equals(installedVersion, repoVersion) ? "You have the latest version!" : "Your version is out of date. Please contact Tom for the latest version.";
	}

	private void ShowDialogIfVersionWrong() 
	{
		if (!installedVersion.Equals(repoVersion))
		{
			Show();
		}
	}

	public override void ApplyAdditionalColours(Color mainColour, Color textColour)
	{
		whatsNewButton.GetComponent<Image>().color = mainColour;
		whatsNewButton.GetComponentInChildren<TMP_Text>().color = textColour;
	}

	/// <summary>
	/// Opens the release page, called from a button.
	/// </summary>
	public void ShowWhatsNew()
	{
		Application.OpenURL(Config.instance.GetEndpoint("repo"));
	}
}
