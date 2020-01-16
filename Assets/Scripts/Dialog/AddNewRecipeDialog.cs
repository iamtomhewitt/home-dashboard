using Requests;
using SimpleJSON;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Dialog
{
	public class AddNewRecipeDialog : Dialog
	{
		[SerializeField] private GameObject newIngredientEntry;
		[SerializeField] private Transform newIngredientsContent;
		[SerializeField] private InputField recipeName;
		[SerializeField] private Text status;

		/// <summary>
		/// Called from a Unity button to add a new ingredient to the add new recipe dialog.
		/// </summary>
		public void AddNewIngredientEntry()
		{
			Instantiate(newIngredientEntry, newIngredientsContent);
		}

		public void UploadNewRecipe()
		{
			StartCoroutine(UploadNewRecipeRoutine());
		}

		private IEnumerator UploadNewRecipeRoutine()
		{
			NewIngredientEntry[] newIngredients = FindObjectsOfType<NewIngredientEntry>();

			JSONObject json = new JSONObject();
			JSONArray ingredientsArray = new JSONArray();

			foreach (NewIngredientEntry newIngredient in newIngredients)
			{
				JSONObject ingredientJson = new JSONObject();
				ingredientJson.Add("name", newIngredient.GetIngredientName());
				ingredientJson.Add("category", newIngredient.GetCategory());
				ingredientJson.Add("amount", newIngredient.GetAmount());
				ingredientJson.Add("weight", newIngredient.GetWeight());
				ingredientsArray.Add(ingredientJson);
			}

			json.Add("name", recipeName.text);
			json.Add("ingredients", ingredientsArray);

			UnityWebRequest request = Postman.CreatePostRequest(Endpoints.RECIPES_ADD, json);
			yield return request.SendWebRequest();

			JSONNode response = JSON.Parse(request.downloadHandler.text);
			status.text = response["message"];
		}

		/// <summary>
		/// Clears out the new ingredients to stop additional ingredients being leaked into the next recipe. Called when the dialog is hidden.
		/// </summary>
		public void RefreshDialog()
		{
			recipeName.text = "";
			status.text = "";

			foreach (NewIngredientEntry i in FindObjectsOfType<NewIngredientEntry>())
			{
				Destroy(i.gameObject);
			}
		}
	}
}
