﻿using Dialog;
using SimpleJSON;
using System.IO;
using UnityEngine;

/// <summary>
/// Access config as JSON via this component. <para/>
/// 
/// E.g. : <code>string value = Config.GetWidgetConfig()["binDay"]["title"]</code>
/// </summary>
public class Config : MonoBehaviour
{
	[SerializeField] private TextAsset cmsConfigFile;
	[SerializeField] private TextAsset configFileTemplate;

	public static Config instance;

	private JSONNode root;

	private string filename = "/config.json";

	private void Awake()
	{
		instance = this;
		string configFilePath = Application.persistentDataPath + filename;

		if (!File.Exists(configFilePath))
		{
			Debug.Log("Could not find config file at this location: " + configFilePath);
			root = JSON.Parse(configFileTemplate.text);
			FindObjectOfType<SettingsDialog>().Show();
		}
		else
		{
			Debug.Log("Using existing config file: " + configFilePath);
			SetRoot(GetFileContents(configFilePath));
		}
	}

	private void SetRoot(string contents)
	{
		root = JSON.Parse(contents);
	}

	public JSONNode GetCmsConfig()
	{
		return JSON.Parse(cmsConfigFile.text);
	}

	public JSONNode GetRoot()
	{
		return root;
	}

	public JSONNode GetWidgetConfig()
	{
		return root["widgets"];
	}

	public JSONNode GetDialogConfig()
	{
		return root["dialogs"];
	}

	public JSONNode GetGeneralConfig()
	{
		return root["general"];
	}

	public string GetEndpoint(string key)
	{
		return root["endpoints"][key].Value;
	}

	public void Replace(JSONNode key, string value)
	{
		key.Value = value;
		SaveToFile(root.ToString());
	}

	public void SaveToFile(string contents)
	{
		string filePath = Application.persistentDataPath + filename;
		StreamWriter writer = new StreamWriter(filePath, false);
		writer.Write(contents);
		writer.Close();
		SetRoot(contents);
	}

	private void CreateNewFile(string filePath)
	{
		StreamWriter writer = new StreamWriter(filePath, true);
		writer.Write(configFileTemplate.text);
		writer.Close();
	}

	private string GetFileContents(string filePath)
	{
		StreamReader reader = new StreamReader(filePath);
		string contents = reader.ReadToEnd();
		reader.Close();
		return contents;
	}

	public string GetRawConfig()
	{
		return GetFileContents(Application.persistentDataPath + filename);
	}
}