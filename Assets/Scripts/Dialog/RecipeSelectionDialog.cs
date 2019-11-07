using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Recipes;
using Recipes.ScriptableObjects;
using Recipes.UI;

namespace Dialog
{
	public class RecipeSelectionDialog : Dialog
	{
		[SerializeField] private GameObject recipeEntryPrefab;
		[SerializeField] private Transform scrollViewContent;

		private Recipe selectedRecipe;

		private string freeTextRecipe;

		public void SelectFreeTextRecipe(InputField freeTextInput)
		{
			freeTextRecipe = freeTextInput.text;
			selectedRecipe = null;
			SetResult(DialogResult.FINISHED);
			Hide();
		}

		/// <summary>
		/// Populates the scroll view with <code>RecipeEntry</code> objects.
		/// </summary>
		public void PopulateRecipes()
		{
			foreach (Transform child in scrollViewContent)
			{
				Destroy(child.gameObject);
			}

			List<string> recipeNames = RecipeDatabase.instance.GetRecipeNames();

			foreach (string recipeName in recipeNames)
			{
				Instantiate(recipeEntryPrefab, scrollViewContent).GetComponent<RecipeEntry>().SetButtonText(recipeName);
			}
		}

		public Recipe GetSelectedRecipe()
		{
			return selectedRecipe;
		}

		public void SetSelectedRecipe(Recipe recipe)
		{
			selectedRecipe = recipe;
		}

		public string GetFreeTextRecipeName()
		{
			return freeTextRecipe;
		}
	}
}