using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using JsonLib;
using TMPro;

namespace Dialog
{
	/// <summary>
	/// A dialog that shows the current settings, which can be updated.
	/// </summary>
	public class SettingsDialog : PopupDialog
	{
		[Header("Settings Dialog Settings")]
		[SerializeField] private GameObject titlePrefab;
		[SerializeField] private GameObject settingPrefab;
		[SerializeField] private GameObject spacerPrefab;
		[SerializeField] private Button saveButton;
		[SerializeField] private Button helpButton;
		[SerializeField] private Image scrollBackground;
		[SerializeField] private Image scrollHandle;
		[SerializeField] private Transform contentParent;

		private List<KeyValuePair<string, string[]>> values = new List<KeyValuePair<string, string[]>>();
		private const string titleId = "<title>";
		private const string subtitleId = "<subtitle>";
		private const string spacerId = "<space>";

		private void Start()
		{
			Screen.sleepTimeout = SleepTimeout.NeverSleep;
			DynamicallyCreateDialog();
		}

		private void DynamicallyCreateDialog()
		{
			string rawConfig = Config.instance.GetRawConfig();
			string lastTitle = "";
			string lastSubtitle = "";

			JSONObject json = new JSONObject(rawConfig);
			CollectJsonData("", json, "");

			foreach (KeyValuePair<string, string[]> kvp in values)
			{
				string key = kvp.Key;
				string value = kvp.Value[0];
				string keyTree = kvp.Value.Length > 1 ? keyTree = kvp.Value[1] : "";

				if (value.Equals(titleId) && !lastTitle.Equals(key))
				{
					lastTitle = key;
					CreateTitle(key);
				}
				else if (value.Equals(subtitleId) && !lastSubtitle.Equals(key))
				{
					lastSubtitle = key;
					CreateSubTitle(key);
				}
				else if (value.Equals(spacerId))
				{
					Instantiate(spacerPrefab, contentParent);
				}
				else
				{
					if (!value.Equals(titleId))
					{
						CreateSetting(key, value, keyTree);
					}
				}
			}
		}

		private void CollectJsonData(string key, JSONObject obj, string keyTree)
		{
			string delimiter = "|";

			if (!keyTree.Contains(delimiter + key + delimiter))
			{
				keyTree += key + delimiter;
			}

			switch (obj.type)
			{
				case JSONObject.Type.OBJECT:
					{
						if (!IsSubtitle(key))
						{
							values.Add(new KeyValuePair<string, string[]>(key, new string[] { titleId, "" }));
						}

						for (int i = 0; i < obj.list.Count; i++)
						{
							string objKey = (string)obj.keys[i];
							JSONObject objJson = (JSONObject)obj.list[i];

							CollectJsonData(objKey, objJson, keyTree);

							if (ShouldAddSpaceAfter(objKey))
							{
								values.Add(new KeyValuePair<string, string[]>(key, new string[] { spacerId }));
							}
						}
						break;
					}

				case JSONObject.Type.ARRAY:
					{
						// All config that has an array is a subtitle
						values.Add(new KeyValuePair<string, string[]>(key, new string[] { subtitleId }));
						int i = 0;
						foreach (JSONObject j in obj.list)
						{
							string kt = keyTree + i + delimiter;
							CollectJsonData(key, j, kt);
							i++;
						}
						break;
					}

				case JSONObject.Type.STRING:
					values.Add(new KeyValuePair<string, string[]>(key, new string[] { obj.str, keyTree }));
					break;

				case JSONObject.Type.NUMBER:
					values.Add(new KeyValuePair<string, string[]>(key, new string[] { obj.n.ToString(), keyTree }));
					break;

				case JSONObject.Type.BOOL:
					values.Add(new KeyValuePair<string, string[]>(key, new string[] { obj.b.ToString(), keyTree }));
					break;

				case JSONObject.Type.NULL:
					Debug.Log("NULL");
					break;
			}
		}

		private bool ShouldAddSpaceAfter(string key)
		{
			return new List<string> { "endPoint", "binColour" }.Contains(key);
		}

		private bool IsSubtitle(string key)
		{
			return new List<string> { "bins", "journeys" }.Contains(key);
		}

		private TMP_Text CreateTitle(string value)
		{
			TMP_Text title = Instantiate(titlePrefab, contentParent).GetComponent<TMP_Text>();
			title.text = Utility.CamelCaseToSentence(value);
			title.gameObject.name = title.text + " Title";
			return title;
		}

		private TMP_Text CreateSubTitle(string value)
		{
			TMP_Text subtitle = CreateTitle(value);
			subtitle.fontSize -= 0.5f;
			return subtitle;
		}

		private void CreateSetting(string key, string value, string keyTree)
		{
			Setting setting = Instantiate(settingPrefab, contentParent).GetComponent<Setting>();
			setting.SetKeyLabel(key.Equals("stops") ? " " : key);
			setting.SetKey(key);
			setting.SetValueInput(value);
			setting.SetValue(value);
			setting.SetKeyTree(keyTree);
			setting.gameObject.name = setting.GetKeyLabel();
		}

		public override void ApplyAdditionalColours(Color mainColour, Color textColour)
		{
			saveButton.GetComponent<Image>().color = mainColour;
			saveButton.GetComponentInChildren<TMP_Text>().color = textColour;

			helpButton.GetComponent<Image>().color = mainColour;
			helpButton.GetComponentInChildren<TMP_Text>().color = textColour;

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
	
		public void OpenHelp()
		{
			Application.OpenURL("https://github.com/iamtomhewitt/home-dashboard/wiki/Config");
		}
	}
}