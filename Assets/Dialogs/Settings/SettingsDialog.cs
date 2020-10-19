using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using SimpleJSON;

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

		private void Start()
		{
			Screen.sleepTimeout = SleepTimeout.NeverSleep;
			infoText.text = infoText.text.Replace("<DASHBOARDKEY>", Config.instance.GetCmsConfig()["cmsApiKey"]);
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
		public void SaveToConfig()
		{
			StartCoroutine(SaveToConfigRoutine());
		}

		private IEnumerator SaveToConfigRoutine()
		{
			ConfirmDialog dialog = FindObjectOfType<ConfirmDialog>();
			dialog.Show();
			dialog.SetNone();
			dialog.SetDialogTitleText("Save config?");
			dialog.SetInfoMessage("This can have unexpected consequences!");

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
				Config config = Config.instance;

				List<Setting> settings = new List<Setting>(FindObjectsOfType<Setting>());
				foreach (Setting setting in settings)
				{
					if (!string.IsNullOrEmpty(setting.GetValue()))
					{
						SimpleJSON.JSONNode node = setting.GetNodeToUpdate();
						config.Replace(node, setting.GetValueInput());
					}
				}

				config.SaveToFile();
			}

			dialog.SetDialogTitleText("Are you sure?");

			foreach (Widget widget in FindObjectsOfType<Widget>())
			{
				widget.Initialise();
			}
		}

		public void OpenConfigServer()
		{
			Application.OpenURL(Config.instance.GetCmsConfig()["cmsUrl"]);
		}
	}
}