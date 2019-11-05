using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Dialog;

public class RecipeEntry : MonoBehaviour
{
	public void SetButtonText(string text)
	{
		GetComponentInChildren<Text>().text = text;
	}

	public string GetButtonText()
	{
		return GetComponentInChildren<Text>().text;
	}

	/// <summary>
	/// Selects a recipe. <para/>
	/// Examines the text of the button (which gets set to the recipe name when the selection dialog is populated),
	/// and finds the associated recipe object from the database. <para/>
	/// Called from a Button.
	/// </summary>
	public void SelectRecipe()
	{
		RecipeSelectionDialog dialog = FindObjectOfType<RecipeSelectionDialog>();
		Recipe selectedRecipe = RecipeDatabase.instance.GetRecipes().Where(r => r.name.Equals(GetButtonText())).First();
		dialog.SetSelectedRecipe(selectedRecipe);
		dialog.SetResult(DialogResult.FINISHED);
		dialog.Hide();
	}
}
