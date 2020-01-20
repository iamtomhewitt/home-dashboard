using SimpleJSON;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A key lavel and value input field on the settings page.
/// </summary>
public class Setting : MonoBehaviour
{
	[SerializeField] private string[] keyTree;
	[SerializeField] private InputField value;

	private void Start()
	{
		JSONNode node = Config.instance.GetConfig();

		// Find the correct node to update
		foreach (string key in keyTree)
		{
			node = node[key];
		}

		value.text = node.Value;
	}

	/// <summary>
	/// Returns the key of the value of the tree.
	/// E.g. apiKeys->googleCalendars->someone@gmail.com would return the key someone@gmail.com
	/// </summary>
	public string[] GetKeyTree()
	{
		return keyTree;
	}

	public string GetValue()
	{
		return value.text;
	}
}