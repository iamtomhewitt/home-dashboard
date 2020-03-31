using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System;
using SimpleJSON;

/// <summary>
/// A key label and value input field on the settings page.
/// </summary>
public class Setting : MonoBehaviour
{
	[SerializeField] private TMP_Text keyLabel;
	[SerializeField] private InputField valueInput;

	private string key;
	private string value;
	private List<string> keyTree;

	private void Start()
	{
		JSONNode node = Config.instance.GetRoot();
		string path = "";

		// Find the correct node to update
		foreach (string key in keyTree)
		{
			int i;
			bool keyIsInt = int.TryParse(key, out i);
			node = keyIsInt ? node[i] : node[key];
			path += key + ",";
		}

		print(node.Value + " (path: " + path + ")");
		valueInput.text = node.Value;
	}

	public void SetKey(string key)
	{
		this.key = key;
	}

	public string GetKey()
	{
		return key;
	}

	public void SetValue(string value)
	{
		this.value = value;
	}

	public string GetValue()
	{
		return value;
	}

	public void SetKeyTree(string keyTreeStringWithDelimeters)
	{
		this.keyTree = new List<string>(keyTreeStringWithDelimeters.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries));
	}

	public List<string> GetKeyTree()
	{
		return keyTree;
	}

	public string GetValueInput()
	{
		return valueInput.text;
	}

	public void SetValueInput(string value)
	{
		this.valueInput.text = value;
		if (this.valueInput.text.Equals("-"))
		{
			this.valueInput.interactable = false;
		}
	}

	public void SetKeyLabel(string text)
	{
		keyLabel.text = Utility.CamelCaseToSentence(text);
	}

	public string GetKeyLabel()
	{
		return keyLabel.text;
	}
}