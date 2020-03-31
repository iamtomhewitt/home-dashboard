﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using JsonLib;
using System.Collections.Generic;

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

		private List<KeyValuePair<string, string>> values = new List<KeyValuePair<string, string>>();

		private void Start()
		{
			Screen.sleepTimeout = SleepTimeout.NeverSleep;
			// DisplayRawConfig();
			Dynamic();
		}

		private void Dynamic()
		{
			string rawConfig = Config.instance.GetRawConfig();

			JSONObject json = new JSONObject(rawConfig);
			collectJsonData("", json);

			string lastTitle = "";

			foreach (KeyValuePair<string, string> kvp in values)
			{
				string key = kvp.Key;
				string value = kvp.Value;

				// print("\t\tLast Key: "+ lastKey + ", currentKey: " + key);

				if (value.Equals("TITLE") && !lastTitle.Equals(key))
				{
					lastTitle = key;
					key = "<b>" + key + "</b>";
					print(key);
				}
				else
				{
					if (!value.Equals("TITLE"))
					{
						print("\t"+key + ":" + value);
					}
				}
			}
		}

		private void collectJsonData(string key, JSONObject obj)
		{
			switch (obj.type)
			{
				case JSONObject.Type.OBJECT:
					values.Add(new KeyValuePair<string, string>(key, "TITLE"));

					for (int i = 0; i < obj.list.Count; i++)
					{
						string k = (string)obj.keys[i];
						JSONObject j = (JSONObject)obj.list[i];
						collectJsonData(k, j);
					}
					break;

				case JSONObject.Type.ARRAY:
					values.Add(new KeyValuePair<string, string>(key, "TITLE"));
					foreach (JSONObject j in obj.list)
					{
						collectJsonData(key, j);
					}
					break;

				case JSONObject.Type.STRING:
					values.Add(new KeyValuePair<string, string>(key, obj.str));
					break;

				case JSONObject.Type.NUMBER:
					values.Add(new KeyValuePair<string, string>(key, obj.n.ToString()));
					break;

				case JSONObject.Type.BOOL:
					values.Add(new KeyValuePair<string, string>(key, obj.b.ToString()));
					break;

				case JSONObject.Type.NULL:
					Debug.Log("NULL");
					break;
			}
		}

		private void DisplayRawConfig()
		{
			//content.text = JsonNode.ParseJsonString(Config.instance.GetRawConfig()).ToJsonPrettyPrintString();
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