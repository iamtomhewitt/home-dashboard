using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The recipes retrieved from Heroku shown on the recipe selection dialog.
/// </summary>
public class RecipeEntry : MonoBehaviour
{
	[SerializeField] private Text text;

	public void SetText(string message)
	{
		text.text = message;
	}

	public string GetText()
	{
		return text.text;
	}
}
