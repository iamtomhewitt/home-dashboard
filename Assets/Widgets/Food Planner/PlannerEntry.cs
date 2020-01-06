using Dialog;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FoodPlannerWidget
{
	public class PlannerEntry : MonoBehaviour
	{
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
				//recipeText.text = dialog.GetSelectedRecipe() != null ? dialog.GetSelectedRecipe().name : dialog.GetFreeTextRecipeName();
				//UpdateRecipeDataName();
				//FindObjectOfType<FoodPlanner>().SaveToFile();
				yield break;
			}
		}
	}
}