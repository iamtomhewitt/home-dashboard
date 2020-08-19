using SimpleJSON;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

/// <summary>
/// A key label and value input field on the settings page.
/// </summary>
public class Setting : MonoBehaviour
{
	[SerializeField] private InputField valueInput;
	[SerializeField] private TMP_Text keyLabel;

	private List<string> keyTree;
	private string key;
	private string value;

	private void Start()
	{
		JSONNode n = GetNodeToUpdate();
		valueInput.text = n.Value;
	}

	public JSONNode GetNodeToUpdate()
	{
		JSONNode node = Config.instance.GetRoot();

		foreach (string key in keyTree)
		{
			int i;
			bool keyIsInt = int.TryParse(key, out i);
			node = keyIsInt ? node[i] : node[key];
		}

		return node;
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