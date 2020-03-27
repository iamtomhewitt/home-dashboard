using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using NiceJson;
using TMPro;

namespace Dialog
{
	/// <summary>
	/// A dialog that shows the current settings, which can be updated.
	/// </summary>
	public class SettingsDialog : PopupDialog
	{
		[SerializeField] private GameObject titlePrefab;
		[SerializeField] private GameObject settingPrefab;
		[SerializeField] private Button saveButton;
		[SerializeField] private Image scrollBackground;
		[SerializeField] private Image scrollHandle;
		[SerializeField] private TMP_InputField content;

		private void Start()
		{
			Screen.sleepTimeout = SleepTimeout.NeverSleep;
			DisplayRawConfig();
		}

		private void DisplayRawConfig()
		{
			content.text = JsonNode.ParseJsonString(Config.instance.GetRawConfig()).ToJsonPrettyPrintString();
		}

		public override void ApplyAdditionalColours(Color mainColour, Color textColour)
		{
			saveButton.GetComponent<Image>().color = mainColour;
			saveButton.GetComponentInChildren<TMP_Text>().color = textColour;

			scrollBackground.color = Colours.Darken(mainColour);
			scrollHandle.color = Colours.Lighten(mainColour);
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
				Config.instance.SaveToFile(content.text);
			}

			dialog.SetDialogTitleText("Are you sure?");

			foreach (Widget widget in FindObjectsOfType<Widget>())
			{
				widget.Initialise();
			}
		}
	}
}