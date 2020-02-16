using System;
using UnityEngine;

namespace SimpleJSON
{
	/// <summary>
	/// Constructs bodys of API requests.
	/// </summary>
	public class JsonBody
	{
		public static JSONObject AddToPlanner(string recipe, string day, string apiKey)
		{
			JSONObject json = new JSONObject();
			json.Add("recipe", recipe);
			json.Add("day", day);
			json.Add("apiKey", apiKey);
			return json;
		}

		public static JSONObject AddRecipe(string name, JSONArray ingredients, string apiKey)
		{
			JSONObject json = new JSONObject();
			json.Add("name", name);
			json.Add("ingredients", ingredients);
			json.Add("apiKey", apiKey);
			return json;
		}

		public static JSONObject RecipeIngredient(string name, double amount, string weight, string category)
		{
			JSONObject json = new JSONObject();
			json.Add("name", name);
			json.Add("amount", amount);
			json.Add("weight", weight);
			json.Add("category", category);
			return json;
		}

		/// <summary>
		/// Has to return a string as SimpleJSON cannot use longs yet.
		/// </summary>
		public static string TodoistTask(string item, string projectId)
		{
			return "{\"content\": \"" + item + "\", \"project_id\": " + projectId + " }";
		}
	}
}