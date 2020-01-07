using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Dialog;
using UnityEngine.Networking;
using System.Text;
using SimpleJSON;

namespace FoodPlannerWidget
{
	public class PlannerEntry : MonoBehaviour
	{
		[SerializeField] private Day day;
		[SerializeField] private Text dayText;
		[SerializeField] private Text recipe;

		private void Start()
		{
			string label = "";
			foreach (char c in day.ToString().Substring(0, 3).ToUpper())
			{
				label += c + "\n";
			}
			dayText.text = label;
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

			while (dialog.GetResult() != DialogResult.FINISHED)
			{
				yield return null;
			}

			if (dialog.GetResult() == DialogResult.FINISHED)
			{
				recipe.text = !string.IsNullOrEmpty(dialog.GetSelectedRecipe()) ? dialog.GetSelectedRecipe() : dialog.GetFreeTextRecipeName();

				// Now update the planner online
				string url = "https://home-dashboard-recipe-manager.herokuapp.com/planner/add";
				JSONObject json = new JSONObject();
				json.Add("recipe", recipe.text);
				json.Add("day", day.ToString());

				UnityWebRequest request = UnityWebRequest.Post(url, "POST");
				byte[] body = Encoding.UTF8.GetBytes(json.ToString());

				request.uploadHandler = new UploadHandlerRaw(body);
				request.downloadHandler = new DownloadHandlerBuffer();
				request.SetRequestHeader("Content-Type", "application/json");

				yield return request.SendWebRequest();
				print(request.downloadHandler.text);

				yield break;
			}
		}

		private enum Day { Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday }
	}
}