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
			root = JSON.Parse(configFile.text);
		}
		else if (File.Exists(filePath))
		{
			print("A config file has already been created at: " + filePath + ", using that one.");
			string contents = GetFileContents(filePath);
			root = JSON.Parse(contents);
		}
		else
		{
			print("A config file has not been specified, creating one.");
			CreateNewFile(filePath);
			root = JSON.Parse(configFileTemplate.text);
		}
	}

	public JSONNode GetConfig()
	{
		return root["widgets"];
	}

	public void Replace(JSONNode key, string value)
	{
		key.Value = value;
		SaveToFile();
	}

	public void SaveToFile()
	{
		string filePath = Application.persistentDataPath + filename;
		StreamWriter writer = new StreamWriter(filePath, false);
		writer.Write(root.ToString());
		writer.Close();
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
}