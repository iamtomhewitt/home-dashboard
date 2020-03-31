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
		[SerializeField] private Image scrollBackground;
		[SerializeField] private Image scrollHandle;
		[SerializeField] private Transform contentParent;

		private List<KeyValuePair<string, string>> values = new List<KeyValuePair<string, string>>();
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
			CollectJsonData("", json);

			foreach (KeyValuePair<string, string> kvp in values)
			{
				string key = kvp.Key;
				string value = kvp.Value;

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
					if (!IsSubtitle(key))
					{
						values.Add(new KeyValuePair<string, string>(key, titleId));
					}

					for (int i = 0; i < obj.list.Count; i++)
					{
						string k = (string)obj.keys[i];
						JSONObject j = (JSONObject)obj.list[i];
						CollectJsonData(k, j);

						if (ShouldAddSpaceAfter(k))
						{
							values.Add(new KeyValuePair<string, string>(key, spacerId));
						}
					}
					break;

				case JSONObject.Type.ARRAY:
					// All config that has an array is a subtitle
					values.Add(new KeyValuePair<string, string>(key, subtitleId));

					foreach (JSONObject j in obj.list)
					{
						CollectJsonData(key, j);
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

		private bool ShouldAddSpaceAfter(string key)
		{
			List<string> ids = new List<string> { "endPoint", "binColour" };
			return ids.Contains(key);
		}

		private bool IsSubtitle(string key)
		{
			List<string> ids = new List<string> { "bins", "journeys" };
			return ids.Contains(key);
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
			subtitle.fontStyle = FontStyles.Italic;
			subtitle.fontSize -= 0.3f;
			subtitle.gameObject.name = subtitle.text + " Subtitle";
			return subtitle;
		}

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