using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using SimpleJSON;

namespace Dialog
{
	public class SettingsDialog : Dialog
	{
		[SerializeField] private Button saveButton;
		[SerializeField] private Image scrollBackground;
		[SerializeField] private Image scrollHandle;

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
					JSONNode node = config.GetWidgetConfig();

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

		/// <summary>
		/// Called from a Unity button.
		/// </summary>
		public void ApplyColours()
		{
			JSONNode config = Config.instance.GetDialogConfig()["settings"];

			Color mainColour = Utils.ToColour(config["mainColour"]);
			Color textColour = Utils.ToColour(config["textColour"]);

			SetTopBarColour(mainColour);
			SetDialogTitleColour(textColour);
			SetHideButtonColour(mainColour, textColour);
			scrollBackground.color = Utils.Darken(mainColour);
			scrollHandle.color = Utils.Lighten(mainColour);
			saveButton.GetComponent<Image>().color = mainColour;
			saveButton.GetComponentInChildren<Text>().color = textColour;
		}
	}
}