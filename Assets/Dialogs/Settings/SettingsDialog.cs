﻿using Planner;
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
		[SerializeField] private Button copyButton;
		[SerializeField] private TMP_Text infoText;
		[SerializeField] private TMP_Text statusText;

		private string cmsApiKey;
		private string cmsApiUrl;
		private string cmsUrl;

		private void Start()
		{
			Screen.sleepTimeout = SleepTimeout.NeverSleep;
			cmsApiKey = Config.instance.GetCmsConfig()["cmsApiKey"];
			cmsUrl = Config.instance.GetCmsConfig()["cmsUrl"];
			cmsApiUrl = Config.instance.GetCmsConfig()["cmsApiUrl"];
			statusText.SetText("");
		}

		public override void ApplyAdditionalColours(Color mainColour, Color textColour)
		{
			downloadConfigButton.GetComponent<Image>().color = mainColour;
			downloadConfigButton.GetComponentInChildren<TMP_Text>().color = textColour;

			manageButton.GetComponent<Image>().color = mainColour;
			manageButton.GetComponentInChildren<TMP_Text>().color = textColour;

			copyButton.GetComponent<Image>().color = mainColour;
			copyButton.GetComponentInChildren<TMP_Text>().color = textColour;
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

			FindObjectOfType<GeneralSettings>().Start();

			this.Hide();
		}

		public void OpenConfigServer()
		{
			Application.OpenURL(cmsUrl + "?token=" + cmsApiKey);
		}

		public void DownloadConfig()
		{
			StartCoroutine(DownloadConfigRoutine());
		}

		private IEnumerator DownloadConfigRoutine()
		{
			statusText.SetText("Please wait...");
			UnityWebRequest request = Postman.CreateGetRequest(this.cmsApiUrl);
			yield return request.SendWebRequest();

			if (request.result != UnityWebRequest.Result.ProtocolError)
			{
				JSONNode json = JSON.Parse(request.downloadHandler.text);
				foreach (JSONNode item in json)
				{
					if (item["token"] == this.cmsApiKey)
					{
						SaveToConfig(item.ToString());
						statusText.SetText("Download success!");
						yield break;
					}
				}
				WidgetLogger.instance.Log("No config was found for " + this.cmsApiKey);
				statusText.SetText("No config found!");
			}
			else
			{
				statusText.SetText("Error: " + request.error);
			}
		}

		public override void PostShow()
		{
			statusText.SetText("");
		}

		/// <summary>
		/// Called from a Unity button.
		/// </summary>
		public void CopyCodeToClipboard()
		{
			TextEditor te = new TextEditor();
			te.text = cmsApiKey;
			te.SelectAll();
			te.Copy();
			statusText.text = "Token '" + cmsApiKey + "' copied to clipboard!";
		}
	}
}