using Planner;
using Requests;
using SimpleJSON;
using System.Collections;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine;

namespace Dialog
{
	/// <summary>
	/// A dialog that shows the current settings, which can be updated.
	/// </summary>
	public class SettingsDialog : PopupDialog
	{
		[Header("Settings Dialog Settings")]
		[SerializeField] private Button manageButton;
		[SerializeField] private Button downloadConfigButton;
		[SerializeField] private TMP_Text infoText;
		[SerializeField] private TMP_Text downloadStatusText;
		[SerializeField] private TMP_InputField apiKeyField;

		private string cmsApiKey;
		private string cmsApiUrl;
		private string cmsUrl;

		private void Start()
		{
			Screen.sleepTimeout = SleepTimeout.NeverSleep;
			cmsApiKey = Config.instance.GetCmsConfig()["cmsApiKey"];
			cmsUrl = Config.instance.GetCmsConfig()["cmsUrl"];
			cmsApiUrl = Config.instance.GetCmsConfig()["cmsApiUrl"];
			apiKeyField.text = cmsApiKey;
			downloadStatusText.SetText("");
		}

		public override void ApplyAdditionalColours(Color mainColour, Color textColour)
		{
			downloadConfigButton.GetComponent<Image>().color = mainColour;
			downloadConfigButton.GetComponentInChildren<TMP_Text>().color = textColour;

			manageButton.GetComponent<Image>().color = mainColour;
			manageButton.GetComponentInChildren<TMP_Text>().color = textColour;
		}

		/// <summary>
		/// Called from a Unity button, saves all the settings objects to the config file.
		/// </summary>
		public void SaveToConfig(string contents)
		{
			StartCoroutine(SaveToConfigRoutine(contents));
		}

		private IEnumerator SaveToConfigRoutine(string contents)
		{
			ConfirmDialog dialog = FindObjectOfType<ConfirmDialog>();
			dialog.Show();
			dialog.SetNone();
			dialog.SetDialogTitleText("Save config?");
			dialog.SetInfoMessage("This can have unexpected consequences!\nChanges will take effect the next time the dashboard loads.");

			while (dialog.IsNone())
			{
				yield return null;
			}

			if (dialog.IsNo())
			{
				dialog.Hide();
				dialog.SetNone();
				yield break;
			}

			if (dialog.IsYes())
			{
				dialog.Hide();
				Config.instance.SaveToFile(contents);
			}

			dialog.SetDialogTitleText("Are you sure?");

			foreach (Widget widget in FindObjectsOfType<Widget>())
			{
				widget.Initialise();
				widget.Run();
			}

			foreach (PlannerEntry entry in FindObjectsOfType<PlannerEntry>())
			{
				entry.Start();
			}

			this.Hide();
		}

		public void OpenConfigServer()
		{
			Application.OpenURL(cmsUrl);
		}

		public void DownloadConfig()
		{
			StartCoroutine(DownloadConfigRoutine());
		}

		private IEnumerator DownloadConfigRoutine()
		{
			downloadStatusText.SetText("");
			UnityWebRequest request = Postman.CreateGetRequest(cmsApiUrl);
			request.SetRequestHeader("x-auth-token", cmsApiKey);
			yield return request.SendWebRequest();

			JSONNode json = JSON.Parse(request.downloadHandler.text);

			if (!request.isHttpError)
			{
				SaveToConfig(json[cmsApiKey].ToString());
				downloadStatusText.SetText("Download success!");
			}
			else
			{
				downloadStatusText.SetText(request.error);
			}
		}

		public override void PostShow()
		{
			downloadStatusText.SetText("");
		}
	}
}