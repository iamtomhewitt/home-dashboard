﻿using UnityEngine;
using UnityEngine.UI;
using Dialog;

/// <summary>
/// The recipes retrieved from Heroku shown on the recipe selection dialog.
/// </summary>
public class RecipeEntry : MonoBehaviour
{
	[SerializeField] private Text recipe;

	private RecipeSelectionDialog dialog;

	public void SetText(string message)
	{
		recipe.text = message;
	}

	public void SetParentDialog(RecipeSelectionDialog dialog)
	{
		this.dialog = dialog;
	}

	/// <summary>
	/// Called when a recipe is selected from the list of available recipes.
	/// </summary>
	public void Select()
	{
		dialog.SelectRecipe(recipe.text);
	}
}