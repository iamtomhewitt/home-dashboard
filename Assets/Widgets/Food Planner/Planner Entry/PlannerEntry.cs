using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using Dialog;
using SimpleJSON;
using Requests;

namespace Planner
{
	public class PlannerEntry : MonoBehaviour
	{
		[SerializeField] private Day day;
		[SerializeField] private Text dayText;
		[SerializeField] private Text recipe;
		[SerializeField] private Image recipeBackground;
		[SerializeField] private Image dayBackground;

		private string configKey;
		private string apiKey;

		private IEnumerator Start()
		{
			configKey = FindObjectOfType<FoodPlanner>().GetWidgetConfigKey();
			apiKey = Config.instance.GetWidgetConfig()[configKey]["apiKey"];
			
			string label = "";
			foreach (char c in day.ToString().Substring(0, 3).ToUpper())
			{
				label += c + "\n";
			}
			dayText.text = label;

			UnityWebRequest request = Postman.CreateGetRequest(Endpoints.PLANNER + "?day=" + day.ToString() + "&apiKey=" + Config.instance.GetWidgetConfig()[configKey]["apiKey"]);
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
			dialog.SetNone();
			dialog.PopulateRecipes();

			while (!dialog.IsFinished() && !dialog.IsCancel())
			{
				yield return null;
			}

			if (dialog.IsFinished())
			{
				recipe.text = !string.IsNullOrEmpty(dialog.GetSelectedRecipe()) ? dialog.GetSelectedRecipe() : dialog.GetFreeTextRecipeName();

				// Now update the planner online
				JSONObject json = new JSONObject();
				json.Add("recipe", string.IsNullOrEmpty(recipe.text) ? " " : recipe.text);
				json.Add("day", day.ToString());
				json.Add("apiKey", apiKey);

				UnityWebRequest request = Postman.CreatePostRequest(Endpoints.PLANNER_ADD, json);
				yield return request.SendWebRequest();

				yield break;
			}
		}

		/// <summary>
		/// Quick method to clear the entry
		/// </summary>
		public void ClearRecipe()
		{
			StartCoroutine(ClearRecipeRoutine());
		}

		private IEnumerator ClearRecipeRoutine()
		{
			JSONObject json = new JSONObject();
			json.Add("recipe", " ");
			json.Add("day", day.ToString());
			json.Add("apiKey", apiKey);

			UnityWebRequest request = Postman.CreatePostRequest(Endpoints.PLANNER_ADD, json);
			yield return request.SendWebRequest();

			JSONNode response = JSON.Parse(request.downloadHandler.text);
			if(response["status"] == 200)
			{
				recipe.text = "";
			}
			else
			{
				WidgetLogger.instance.Log("Could not clear recipe: " + response["message"]);
			}
		}

		public string GetRecipeName()
		{
			return recipe.text;
		}

		public void SetRecipeTextColour(Color colour)
		{
			recipe.color = colour;
		}

		public void SetDayTextColour(Color colour)
		{
			dayText.color = colour;
		}

		public void SetRecipeBackgroundColour(Color colour)
		{
			recipeBackground.color = colour;
		}

		public void SetDayBackgroundColour(Color colour)
		{
			dayBackground.color = colour;
		}

		private enum Day { Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday }
	}
}