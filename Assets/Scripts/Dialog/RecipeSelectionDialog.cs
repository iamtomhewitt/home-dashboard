using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using SimpleJSON;
using Requests;

namespace Dialog
{
	public class RecipeSelectionDialog : Dialog
	{
		[SerializeField] private RecipeEntry recipeEntryPrefab;
		[SerializeField] private Transform scrollViewContent;

		private string selectedRecipe;
		private string freeTextRecipe;

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

			UnityWebRequest request = Postman.CreateGetRequest(Endpoints.RECIPES);
			yield return request.SendWebRequest();
			string response = request.downloadHandler.text;

			JSONNode json = JSON.Parse(response);
			for (int i = 0; i < json["recipes"].AsArray.Count; i++)
			{
				RecipeEntry entry = Instantiate(recipeEntryPrefab.gameObject, scrollViewContent).GetComponent<RecipeEntry>();
				entry.SetText(json["recipes"][i]["name"]);
				entry.SetParentDialog(this);
			}
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
	}
}