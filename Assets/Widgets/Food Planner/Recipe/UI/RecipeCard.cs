using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Dialog;
using FoodPlannerWidget;

namespace Recipes.UI
{
	public class RecipeCard : MonoBehaviour
	{
		[SerializeField] private RecipeData recipeData;
		[SerializeField] private Text recipeText;

		/// <summary>
		/// Updates the recipe data objects with the name of the recipe card text.
		/// </summary>
		public void UpdateRecipeDataName()
		{
			recipeData.recipeName = recipeText.text;
		}

		public RecipeData GetRecipeData()
		{
			return recipeData;
		}

		public Text GetRecipeCardText()
		{
			return recipeText;
		}

		/// <summary>
		/// Called from a Button.
		/// </summary>
		public void SelectRecipe()
		{
			StartCoroutine(SelectRecipeRoutine());
		}

		public IEnumerator SelectRecipeRoutine()
		{
			RecipeSelectionDialog dialog = FindObjectOfType<RecipeSelectionDialog>();
			dialog.Show();
			dialog.SetResult(DialogResult.NONE);
			dialog.PopulateRecipes();

			while (dialog.GetResult() != DialogResult.FINISHED)
			{
				yield return null;
			}

			if (dialog.GetResult() == DialogResult.FINISHED)
			{
				recipeText.text = dialog.GetSelectedRecipe() != null ? dialog.GetSelectedRecipe().name : dialog.GetFreeTextRecipeName();
				UpdateRecipeDataName();
				FindObjectOfType<FoodPlanner>().SaveToFile();
				yield break;
			}
		}
	}

	[System.Serializable]
	public class RecipeData
	{
		public string day;
		public string recipeName;
	}
}