using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using SimpleJSON;
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
		[SerializeField] private Transform contentParent;
		[SerializeField] private Button saveButton;
		[SerializeField] private Image scrollBackground;
		[SerializeField] private Image scrollHandle;

		private void Start()
		{
			DynamicallyCreateDialog();
		}

		private void DynamicallyCreateDialog()
		{
			JSONNode widgets = Config.instance.GetWidgetConfig();
			JSONNode dialogs = Config.instance.GetDialogConfig();

			// Populate widgets
			foreach (KeyValuePair<string, JSONNode> widget in (JSONObject)widgets)
			{
				string widgetKey = widget.Key;

				foreach (KeyValuePair<string, JSONNode> kvp in (JSONObject)widget)
				{
					string key = kvp.Key;
					string value = kvp.Value;
					string[] keyTree = new string[] { widgetKey, key };

					if (key.Equals("title"))
					{
						CreateTitle(value);
					}

					CreateSetting(keyTree, key, value, true);

					foreach (JSONNode p in kvp.Value)
					{
						print(p);
						foreach (KeyValuePair<string, JSONNode> k in (JSONObject)p)
						{
							print(k.Key);
						}
					}
				}
			}

			// Populate dialogs
			foreach (KeyValuePair<string, JSONNode> dialog in (JSONObject)dialogs)
			{
				string dialogKey = dialog.Key;
				
				CreateTitle(Utility.CamelCaseToSentence(dialogKey) + " Dialog");

				foreach (KeyValuePair<string, JSONNode> kvp in (JSONObject)dialog)
				{
					CreateSetting(new string[] { dialogKey, kvp.Key }, kvp.Key, kvp.Value, false);
				}
			}
		}

		/// <summary>
		/// Creates a header based on the config.
		/// </summary>
		private void CreateTitle(string value)
		{
			TMP_Text title = Instantiate(titlePrefab, contentParent).GetComponent<TMP_Text>();
			title.text = value;
			title.gameObject.name = title.text + " Title";
		}

		/// <summary>
		/// Creates a key value setting based on the config.
		/// </summary>
		private void CreateSetting(string[] keyTree, string key, string value, bool isWidgetSetting)
		{
			Setting setting = Instantiate(settingPrefab, contentParent).GetComponent<Setting>();
			setting.SetKeyTree(keyTree);
			setting.SetKeyLabel(key);
			setting.SetValue(value);
			setting.SetWidgetSetting(isWidgetSetting);
			setting.gameObject.name = setting.GetKeyLabel();
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
			Config config = Config.instance;
			List<Setting> settings = new List<Setting>(FindObjectsOfType<Setting>());
			List<Widget> widgets = new List<Widget>(FindObjectsOfType<Widget>());

			foreach (Setting setting in settings)
			{
				if (!string.IsNullOrEmpty(setting.GetValue()))
				{
					JSONNode node = setting.IsWidgetSetting() ? config.GetWidgetConfig() : config.GetDialogConfig();

					// Find the correct node to update
					foreach (string key in setting.GetKeyTree())
					{
						node = node[key];
					}

					config.Replace(node, setting.GetValue());
				}
			}

			foreach (Widget widget in widgets)
			{
				widget.Initialise();
			}
		}
	}
}