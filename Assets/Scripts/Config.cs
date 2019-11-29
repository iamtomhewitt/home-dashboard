using UnityEngine;
using SimpleJSON;

/// <summary>
/// Access config as JSON via this component. <para/>
/// 
/// E.g. : <code>string key = Config.GetConfig()["apiKeys"]["keyName"]</code>
/// </summary>
public class Config : MonoBehaviour
{
	[SerializeField] private TextAsset configFile;

	public static Config instance;

	private JSONNode root;

	private void Awake()
	{
		instance = this;
		root = JSON.Parse(configFile.text);
	}

	public JSONNode GetConfig()
	{
		return root;
	}
}
