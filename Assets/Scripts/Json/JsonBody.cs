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
	}
}