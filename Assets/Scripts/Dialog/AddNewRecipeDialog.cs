using SimpleJSON;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Dialog
{
	public class AddNewRecipeDialog : MonoBehaviour
	{
		[SerializeField] private GameObject newIngredientEntry;
		[SerializeField] private Transform newIngredientsContent;
		[SerializeField] private InputField recipeName;

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
			string url = "https://home-dashboard-recipe-manager.herokuapp.com/recipes/add";

			NewIngredientEntry[] newIngredients = FindObjectsOfType<NewIngredientEntry>();

			JSONObject jsonData = new JSONObject();
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

			jsonData.Add("name", recipeName.text);
			jsonData.Add("ingredients", ingredientsArray);

			UnityWebRequest request = UnityWebRequest.Post(url, "POST");
			byte[] body = Encoding.UTF8.GetBytes(jsonData.ToString());

			request.uploadHandler = new UploadHandlerRaw(body);
			request.downloadHandler = new DownloadHandlerBuffer();
			request.SetRequestHeader("Content-Type", "application/json");

			yield return request.SendWebRequest();
			string response = request.downloadHandler.text;
		}
	}
}
