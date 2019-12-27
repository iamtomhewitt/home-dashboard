using System.Collections.Generic;
using UnityEngine;

namespace Dialog
{
	public class SettingsDialog : Dialog
	{
		/// <summary>
		/// Called from a Unity button, saves all the settings objects to the config file.
		/// </summary>
		public void Save()
		{
			Config config = Config.instance;
			List<Setting> settings = new List<Setting>(FindObjectsOfType<Setting>());

			foreach (Setting setting in settings)
			{
				print("Key: " + setting.GetKey());
				print("Value: " + setting.GetValue());
			}
		}
	}
}