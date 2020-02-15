using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;
using System.Text.RegularExpressions;

/// <summary>
/// A key label and value input field on the settings page.
/// </summary>
public class Setting : MonoBehaviour
{
	[SerializeField] private Text keyLabel;
	[SerializeField] private InputField value;
	[SerializeField] private bool widgetSetting;
	[SerializeField] private string[] keyTree;

	private void Start()
	{
		JSONNode node = widgetSetting ? Config.instance.GetWidgetConfig() : Config.instance.GetDialogConfig();

		// Find the correct node to update
		foreach (string key in keyTree)
		{
			node = node[key];
		}

		value.text = node.Value;
	}
	
	public string[] GetKeyTree()
	{
		return keyTree;
	}

	public void SetKeyTree(string[] tree)
	{
		keyTree = tree;
	}

	public string GetValue()
	{
		return value.text;
	}

	public void SetValue(string value)
	{
		this.value.text = value;
	}

	public void SetKeyLabel(string text)
	{
		keyLabel.text = Utility.CamelCaseToSentence(text);
	}

	public string GetKeyLabel()
	{
		return keyLabel.text;
	}

	public bool IsWidgetSetting()
	{
		return widgetSetting;
	}

	public void SetWidgetSetting(bool widgetSetting)
	{
		this.widgetSetting = widgetSetting;
	}
}