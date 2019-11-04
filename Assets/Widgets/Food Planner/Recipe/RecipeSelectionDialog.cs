using UnityEngine;
using System.Collections.Generic;

public class RecipeSelectionDialog : Dialog
{
	[SerializeField] private GameObject recipeEntry;
	[SerializeField] private Transform scrollParent;

	private Recipe selectedRecipe;

	public Recipe GetSelectedRecipe()
	{
		return selectedRecipe;
	}

	public void SetSelectedRecipe(Recipe recipe)
	{
		selectedRecipe = recipe;
	}

	/// <summary>
	/// Populates the scroll view with <code>RecipeEntry</code> objects.
	/// </summary>
	public void PopulateRecipes()
	{
		ClearRecipes();
		ListRecipes();
	}

	/// <summary>
	/// Removes all the gameobjetcs under the scroll view, effectively resetting the list to nothing.
	/// </summary>
	private void ClearRecipes()
	{
		foreach (Transform child in scrollParent)
		{
			Destroy(child.gameObject);
		}
	}

	/// <summary>
	/// Adds recipe entries to the scroll view.
	/// </summary>
	private void ListRecipes()
	{
		List<string> recipeNames = RecipeDatabase.instance.GetRecipeNames();

		foreach (string recipeName in recipeNames)
		{
			Instantiate(recipeEntry, scrollParent).GetComponent<RecipeEntry>().SetButtonText(recipeName);
		}
	}
}
