﻿using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using Requests;
using SimpleJSON;

namespace Dialog
{
	public class AddNewRecipeDialog : PopupDialog
	{
		[Header("Add New Recipe Dialog Settings")]
		[SerializeField] private Button addRecipeButton;
		[SerializeField] private Button addIngredientButton;
		[SerializeField] private Image scrollBackground;
		[SerializeField] private Image scrollHandle;
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

		public override void ApplyAdditionalColours(Color mainColour, Color textColour)
		{
			addIngredientButton.GetComponent<Image>().color = mainColour;
			addIngredientButton.GetComponentInChildren<Text>().color = textColour;

			addRecipeButton.GetComponent<Image>().color = mainColour;
			addRecipeButton.GetComponentInChildren<Text>().color = textColour;

			scrollBackground.color = Colours.Darken(mainColour);
			scrollHandle.color = Colours.Lighten(mainColour);
		}
	}
}
