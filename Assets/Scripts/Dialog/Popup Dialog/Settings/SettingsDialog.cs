﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using SimpleJSON;

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

			foreach (KeyValuePair<string, JSONNode> widget in (JSONObject)widgets)
			{
				string widgetKey = widget.Key;

				foreach (KeyValuePair<string, JSONNode> kvp in (JSONObject)widget)
				{
					string key = kvp.Key;
					string value = kvp.Value;
					string[] keyTree = new string[] { widgetKey, key };
				}
			}
		}

		public override void ApplyAdditionalColours(Color mainColour, Color textColour)
		{
			saveButton.GetComponent<Image>().color = mainColour;
			saveButton.GetComponentInChildren<Text>().color = textColour;

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