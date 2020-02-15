namespace Requests
{
	public class Endpoints
	{
		private static readonly string RECIPE_MANAGER = "https://home-dashboard-recipe-manager.herokuapp.com";
		
		public static readonly string TODOIST_TASKS = "https://api.todoist.com/rest/v1/tasks";
		public static readonly string RECIPES_ADD = RECIPE_MANAGER + "/recipes/add";
		public static readonly string PLANNER = RECIPE_MANAGER + "/planner";
		public static readonly string PLANNER_ADD = RECIPE_MANAGER + "/planner/add";

		public static string RECIPES(string apiKey)
		{
			return RECIPE_MANAGER + "/recipes?apiKey=" + apiKey;
		}

		public static string TRAIN_DEPARTURES(string stationCode, int numberOfResults, string apiKey)
		{
			return "https://home-dashboard-train-manager.herokuapp.com/departures?stationCode=" + stationCode + "&numberOfResults=" + numberOfResults + "&apiKey=" + apiKey;
		}

		public static string BBC_NEWS(string apiKey)
		{
			return "https://newsapi.org/v2/top-headlines?sources=bbc-news&apiKey=" + apiKey;
		}

		public static string GOOGLE_CALENDAR(string gmailAddress, string future, string today, string apiKey)
		{
			return "https://www.googleapis.com/calendar/v3/calendars/" + gmailAddress + "/events?orderBy=startTime&singleEvents=true&timeMax=" + future + "T10:00:00-07:00&timeMin=" + today + "T10:00:00-07:00&key=" + apiKey;
		}

		public static string TODOIST_PROJECT(string id)
		{
			return "https://api.todoist.com/rest/v1/tasks?project_id=" + id;
		}

		public static string WEATHER(string apiKey, string latitude, string longitude)
		{
			return "https://home-dashboard-weather-manager.herokuapp.com/weather?apiKey=" + apiKey + "&latitude=" + latitude + "&longitude=" + longitude;
		}

		public static string SPLITWISE(string groupId, string apiKey)
		{
			return "https://home-dashboard-splitwise-mngr.herokuapp.com/group?groupId=" + groupId + "&apiKey=" + apiKey;
		}
	}
}