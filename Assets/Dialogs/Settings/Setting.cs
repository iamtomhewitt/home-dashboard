﻿using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;
using TMPro;

/// <summary>
/// A key label and value input field on the settings page.
/// </summary>
public class Setting : MonoBehaviour
{
	[SerializeField] private TMP_Text keyLabel;
	[SerializeField] private InputField valueInput;

	private string key;
	private string value;
	private string keyTree;

	private void Start()
	{
		// print("TODO: Figure out keyTree");
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

	public void SetKeyTree(string keyTree)
	{
		this.keyTree = keyTree;
	}

	public string GetKeyTree()
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