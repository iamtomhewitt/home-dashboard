﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Dialog;
using UnityEngine.Networking;
using SimpleJSON;
using Requests;

namespace FoodPlannerWidget
{
	public class PlannerEntry : MonoBehaviour
	{
		[SerializeField] private Day day;
		[SerializeField] private Text dayText;
		[SerializeField] private Text recipe;

		private IEnumerator Start()
		{
			string label = "";
			foreach (char c in day.ToString().Substring(0, 3).ToUpper())
			{
				label += c + "\n";
			}
			dayText.text = label;

			JSONObject json = new JSONObject();
			json.Add("day", day.ToString());

			UnityWebRequest request = Postman.CreateGetRequest(RecipeManagerEndpoints.PLANNER + "?day=" + day.ToString());
			yield return request.SendWebRequest();

			recipe.text = JSON.Parse(request.downloadHandler.text)["planner"]["recipe"];
		}

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

			while (dialog.GetResult() != DialogResult.FINISHED && dialog.GetResult() != DialogResult.CANCEL)
			{
				yield return null;
			}

			if (dialog.GetResult() == DialogResult.FINISHED)
			{
				recipe.text = !string.IsNullOrEmpty(dialog.GetSelectedRecipe()) ? dialog.GetSelectedRecipe() : dialog.GetFreeTextRecipeName();

				// Now update the planner online
				JSONObject json = new JSONObject();
				json.Add("recipe", string.IsNullOrEmpty(recipe.text) ? " " : recipe.text);
				json.Add("day", day.ToString());

				UnityWebRequest request = Postman.CreatePostRequest(RecipeManagerEndpoints.PLANNER_ADD, json);
				yield return request.SendWebRequest();

				yield break;
			}
		}

		public string GetRecipeName()
		{
			return recipe.text;
		}

		private enum Day { Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday }
	}
}