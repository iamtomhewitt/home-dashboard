using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Dialog;
using System;

namespace FoodPlannerWidget
{
	public class PlannerEntry : MonoBehaviour
	{
		[SerializeField] private Text recipe;

		/// <summary>
		/// Called from a Unity button when the planner entry is clicked on.
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
				recipe.text = !string.IsNullOrEmpty(dialog.GetSelectedRecipe()) ? dialog.GetSelectedRecipe() : dialog.GetFreeTextRecipeName();
				yield break;
			}
		}
	}
}