using Planner;
using Requests;
using SimpleJSON;
using System.Collections;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine;

namespace Dialog
{
	/// <summary>
	/// A dialog that shows all the available recipes to choose from when selecting a recipe for a planner entry on the food planner.
	/// </summary>
	public class RecipeSelectionDialog : PopupDialog
	{
		[Header("Recipe Selection Dialog Settings")]
		[SerializeField] private Button addRecipeButton;
		[SerializeField] private Button freeTextButton;
		[SerializeField] private Image freeTextInput;
		[SerializeField] private Image scrollBackground;
		[SerializeField] private Image scrollHandle;
		[SerializeField] private RecipeEntry recipeEntryPrefab;
		[SerializeField] private TMP_Text loadingText;
		[SerializeField] private Transform scrollViewContent;

		private string freeTextRecipe;
		private string selectedRecipe;

		/// <summary>
		/// Populates the scroll view with recipes retrieved from the recipe manager on Heroku.
		/// </summary>
		public void PopulateRecipes()
		{
			StartCoroutine(PopulateRecipesRoutine());
		}

		private IEnumerator PopulateRecipesRoutine()
		{
			ClearExistingRecipes();

			loadingText.text = "Loading...";

			UnityWebRequest request = Postman.CreateGetRequest(Endpoints.instance.RECIPES());
			yield return request.SendWebRequest();
			string response = request.downloadHandler.text;

			JSONNode json = JSON.Parse(response);

			foreach (JSONNode recipe in json)
			{
				RecipeEntry entry = Instantiate(recipeEntryPrefab.gameObject, scrollViewContent).GetComponent<RecipeEntry>();
				entry.SetIngredients(recipe["ingredients"].AsArray);
				entry.SetParentDialog(this);
				entry.SetRecipeText(recipe["name"]);
				entry.SetSteps(recipe["steps"].AsArray);
			}

			loadingText.text = "";
		}

		/// <summary>
		/// Stops duplicate recipes showing.
		/// </summary>
		private void ClearExistingRecipes()
		{
			foreach (Transform child in scrollViewContent)
			{
				Destroy(child.gameObject);
			}
		}

		public void SelectFreeTextRecipe(InputField freeTextInput)
		{
			freeTextRecipe = freeTextInput.text;
			selectedRecipe = "";
			freeTextInput.text = "";
			SetFinished();
			Hide();
		}

		public void SelectRecipe(string recipe)
		{
			freeTextRecipe = "";
			selectedRecipe = recipe;
			SetFinished();
			Hide();
		}

		public string GetFreeTextRecipeName()
		{
			return freeTextRecipe;
		}

		public string GetSelectedRecipe()
		{
			return selectedRecipe;
		}

		public void HideAndCancelResult()
		{
			Hide();
			SetCancel();
		}

		public override void ApplyAdditionalColours(Color mainColour, Color textColour)
		{
			freeTextButton.GetComponent<Image>().color = mainColour;
			freeTextButton.GetComponentInChildren<TMP_Text>().color = textColour;

			addRecipeButton.GetComponent<Image>().color = mainColour;
			addRecipeButton.GetComponentInChildren<TMP_Text>().color = textColour;

			freeTextInput.color = mainColour;

			scrollBackground.color = Colours.Darken(mainColour);
			scrollHandle.color = Colours.Lighten(mainColour);
		}

		public void ShowAddNewRecipeDialog()
		{
			FindObjectOfType<AddNewRecipeDialog>().Show();
		}
	}
}