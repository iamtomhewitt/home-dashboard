using SimpleJSON;
using System.Collections.Generic;

namespace Dialog
{
	public class SettingsDialog : Dialog
	{
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
	}
}