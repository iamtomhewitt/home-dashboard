using UnityEngine;
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
		[SerializeField] private Transform contentParent;

		private List<KeyValuePair<string, string>> values = new List<KeyValuePair<string, string>>();

		private void Start()
		{
			Screen.sleepTimeout = SleepTimeout.NeverSleep;
			DynamicallyCreateDialog();
		}

		private void DynamicallyCreateDialog()
		{
			string rawConfig = Config.instance.GetRawConfig();

			JSONObject json = new JSONObject(rawConfig);
			CollectJsonData("", json);

			string lastTitle = "";

			foreach (KeyValuePair<string, string> kvp in values)
			{
				string key = kvp.Key;
				string value = kvp.Value;

				if (value.Equals("TITLE") && !lastTitle.Equals(key))
				{
					lastTitle = key;
					// key = "<b>" + key + "</b>";
					// print(key);
					CreateTitle(key);
				}
				else
				{
					if (!value.Equals("TITLE"))
					{
						// print("\t" + key + ":" + value);
						CreateSetting(key, value);
					}
				}
			}
		}

		private void CollectJsonData(string key, JSONObject obj)
		{
			switch (obj.type)
			{
				case JSONObject.Type.OBJECT:
					values.Add(new KeyValuePair<string, string>(key, "TITLE"));

					for (int i = 0; i < obj.list.Count; i++)
					{
						string k = (string)obj.keys[i];
						JSONObject j = (JSONObject)obj.list[i];
						CollectJsonData(k, j);
					}
					break;

				case JSONObject.Type.ARRAY:
					values.Add(new KeyValuePair<string, string>(key, "TITLE"));

					foreach (JSONObject j in obj.list)
					{
						CollectJsonData(key, j);
						// values.Add(new KeyValuePair<string, string>("-", "-"));
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

		/// <summary>
		/// Creates a header based on the config.
		/// </summary>
		private void CreateTitle(string value)
		{
			TMP_Text title = Instantiate(titlePrefab, contentParent).GetComponent<TMP_Text>();
			title.text = Utility.CamelCaseToSentence(value);
			title.gameObject.name = title.text + " Title";
		}

		/// <summary>
		/// Creates a key value setting based on the config.
		/// </summary>
		private void CreateSetting(string key, string value)
		{
			Setting setting = Instantiate(settingPrefab, contentParent).GetComponent<Setting>();
			// setting.SetKeyTree(keyTree);
			setting.SetKeyLabel(key);
			setting.SetValue(value);
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
				// Config.instance.SaveToFile(content.text);
			}

			dialog.SetDialogTitleText("Are you sure?");

			foreach (Widget widget in FindObjectsOfType<Widget>())
			{
				widget.Initialise();
			}
		}
	}
}