using UnityEngine;
using UnityEngine.UI;

namespace Dialog
{
	public class RecipeSelectionDialog : Dialog
	{
		[SerializeField] private GameObject recipeEntryPrefab;
		[SerializeField] private Transform scrollViewContent;

		private string freeTextRecipe;

		public void SelectFreeTextRecipe(InputField freeTextInput)
		{
			freeTextRecipe = freeTextInput.text;
			SetResult(DialogResult.FINISHED);
			Hide();
		}

		/// <summary>
		/// Populates the scroll view with <code>RecipeEntry</code> objects.
		/// </summary>
		public void PopulateRecipes()
		{
			print("TODO");
		}

		public string GetFreeTextRecipeName()
		{
			return freeTextRecipe;
		}
	}
}