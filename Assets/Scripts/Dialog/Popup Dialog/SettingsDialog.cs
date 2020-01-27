using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using SimpleJSON;

namespace Dialog
{
	public class SettingsDialog : PopupDialog
	{
		[SerializeField] private Button saveButton;
		[SerializeField] private Image scrollBackground;
		[SerializeField] private Image scrollHandle;

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