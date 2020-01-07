using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using SimpleJSON;

namespace Dialog
{
	public class RecipeSelectionDialog : Dialog
	{
		[SerializeField] private RecipeEntry recipeEntryPrefab;
		[SerializeField] private Transform scrollViewContent;

		private string freeTextRecipe;

		public void SelectFreeTextRecipe(InputField freeTextInput)
		{
			freeTextRecipe = freeTextInput.text;
			SetResult(DialogResult.FINISHED);
			Hide();
		}

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

			string url = "https://home-dashboard-recipe-manager.herokuapp.com/recipes";

			UnityWebRequest request = UnityWebRequest.Get(url);
			yield return request.SendWebRequest();
			string response = request.downloadHandler.text;

			JSONNode json = JSON.Parse(response);

			for (int i = 0; i < json["recipes"].AsArray.Count; i++)
			{
				Instantiate(recipeEntryPrefab.gameObject, scrollViewContent).GetComponent<RecipeEntry>().SetText(json["recipes"][i]["name"]);
			}
		}

		private void ClearExistingRecipes()
		{
			foreach (Transform child in scrollViewContent)
			{
				Destroy(child.gameObject);
			}
		}

		public string GetFreeTextRecipeName()
		{
			return freeTextRecipe;
		}
	}
}