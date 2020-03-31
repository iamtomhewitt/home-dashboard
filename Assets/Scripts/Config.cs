using UnityEngine;
using SimpleJSON;
using System.IO;

/// <summary>
/// Access config as JSON via this component. <para/>
/// 
/// E.g. : <code>string key = Config.GetConfig()["apiKeys"]["keyName"]</code>
/// </summary>
public class Config : MonoBehaviour
{
	[SerializeField] private TextAsset configFile;
	[SerializeField] private TextAsset configFileTemplate;

	public static Config instance;

	private JSONNode root;

	private string filename = "/config.json";

	private void Awake()
	{
		instance = this;
		string filePath = Application.persistentDataPath + filename;

		if (configFile != null)
		{
			Debug.Log("A config file has been supplied on start up, overwriting config...");
			root = JSON.Parse(configFile.text);
			SaveToFile(root.ToString());
			SetRoot(filePath);
		}
		else if (File.Exists(filePath))
		{
			Debug.Log("A config file has already been created at: " + filePath + ", using that one.");
			SetRoot(filePath);
		}
		else
		{
			Debug.Log("A config file has not been specified, creating one.");
			CreateNewFile(filePath);
			SetRoot(filePath);
		}
	}

	private void SetRoot(string filePath)
	{
		string contents = GetFileContents(filePath);
		root = JSON.Parse(contents);
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

	public string GetEndpoint(string key)
	{
		return root["endpoints"][key].Value;
	}

	public void SaveToFile(string contents)
	{
		string filePath = Application.persistentDataPath + filename;
		StreamWriter writer = new StreamWriter(filePath, false);
		writer.Write(contents);
		writer.Close();
		SetRoot(filePath);
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
		return reader.ReadToEnd();
	}

	public string GetRawConfig()
	{
		return GetFileContents(Application.persistentDataPath + filename);
	}
}