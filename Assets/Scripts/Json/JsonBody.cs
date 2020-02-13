using SimpleJSON;

namespace Json
{
	public class JsonBody
	{
		public static JSONObject PLANNER_ADD(string recipe, string day, string apiKey)
		{
			JSONObject json = new JSONObject();
			json.Add("recipe", " ");
			json.Add("day", day);
			json.Add("apiKey", apiKey);
			return json;
		}

		public static JSONObject RECIPE_ADD(string name, JSONArray ingredients, string apiKey)
		{
			JSONObject json = new JSONObject();
			json.Add("name", name);
			json.Add("ingredients", ingredients);
			json.Add("apiKey", apiKey);
			return json;
		}

		public static JSONObject RECIPE_INGREDIENT(string name, double amount, string weight, string category)
		{
			JSONObject json = new JSONObject();
			json.Add("name", name);
			json.Add("amount", amount);
			json.Add("weight", weight);
			json.Add("category", category);
			return json;
		}
	}
}