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
	/// A dialog that adds a new recipe that can be used by the Food Planner.
	/// </summary>
	public class AddNewRecipeDialog : PopupDialog
	{
		[Header("Add New Recipe Dialog Settings")]
		[SerializeField] private Button addIngredientButton;
		[SerializeField] private Button addRecipeButton;
		[SerializeField] private GameObject newIngredientEntry;
		[SerializeField] private Image scrollBackground;
		[SerializeField] private Image scrollHandle;
		[SerializeField] private InputField recipeName;
		[SerializeField] private TMP_Text status;
		[SerializeField] private Transform newIngredientsContent;

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

			JSONArray ingredientsArray = new JSONArray();
			foreach (NewIngredientEntry newIngredient in newIngredients)
			{
				JSONObject ingredientJson = JsonBody.RecipeIngredient(newIngredient.GetIngredientName(), newIngredient.GetAmount(), newIngredient.GetWeight(), newIngredient.GetCategory());
				ingredientsArray.Add(ingredientJson);
			}

			string apiKey = Config.instance.GetWidgetConfig()[FindObjectOfType<FoodPlanner>().GetWidgetConfigKey()]["apiKey"];
			JSONObject body = JsonBody.AddRecipe(recipeName.text, ingredientsArray, apiKey);

			UnityWebRequest request = Postman.CreatePostRequest(Endpoints.instance.RECIPES_ADD(), body);
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

		public override void ApplyAdditionalColours(Color mainColour, Color textColour)
		{
			addIngredientButton.GetComponent<Image>().color = mainColour;
			addIngredientButton.GetComponentInChildren<TMP_Text>().color = textColour;

			addRecipeButton.GetComponent<Image>().color = mainColour;
			addRecipeButton.GetComponentInChildren<TMP_Text>().color = textColour;

			scrollBackground.color = Colours.Darken(mainColour);
			scrollHandle.color = Colours.Lighten(mainColour);
		}

		public void ShowSelectRecipeDialog()
		{
			RecipeSelectionDialog dialog = FindObjectOfType<RecipeSelectionDialog>();
			dialog.Show();
			dialog.PopulateRecipes();
		}
	}
}
