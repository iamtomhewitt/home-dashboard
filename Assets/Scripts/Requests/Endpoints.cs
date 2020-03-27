using System.Collections.Generic;

namespace Requests
{
	public class Endpoints
	{
		private static readonly string RECIPE_MANAGER = "https://home-dashboard-recipe-manager.herokuapp.com";

		public static readonly string TODOIST_TASKS = "https://api.todoist.com/rest/v1/tasks";
		public static readonly string RECIPES_ADD = RECIPE_MANAGER + "/recipes/add";
		public static readonly string PLANNER_ADD = RECIPE_MANAGER + "/planner/add";

		public static string PLANNER(string day, string plannerId, string apiKey)
		{
			string s = string.Format("{0}/planner?apiKey={1}&plannerId={2}", RECIPE_MANAGER, apiKey, plannerId);
			if (!string.IsNullOrEmpty(day))
			{
				s += string.Format("&day={0}", day);
			}
			return s;
		}

		public static string RECIPES(string apiKey)
		{
			return string.Format("{0}/recipes?apiKey={1}", RECIPE_MANAGER, apiKey);
		}

		public static string TRAIN_DEPARTURES(string stationCode, int numberOfResults, string apiKey)
		{
			return string.Format("https://huxley.apphb.com/departures/{0}/{1}?accessToken={2}", stationCode, numberOfResults, apiKey);
		}

		public static string BBC_NEWS(string apiKey)
		{
			return string.Format("https://newsapi.org/v2/top-headlines?sources=bbc-news&apiKey={0}", apiKey);
		}

		public static string GOOGLE_CALENDAR(string apiKey, string gmailAddress, int numberOfEvents)
		{
			return string.Format("https://dashboard-calendar-manager.herokuapp.com/calendar/events?apiKey={0}&gmailAddress={1}&numberOfEvents={2}", apiKey, gmailAddress, numberOfEvents);
		}

		public static string TODOIST_PROJECT(string id)
		{
			return string.Format("https://api.todoist.com/rest/v1/tasks?project_id={0}", id);
		}

		public static string WEATHER(string apiKey, string latitude, string longitude)
		{
			return string.Format("https://api.darksky.net/forecast/{0}/{1},{2}?units=uk", apiKey, latitude, longitude);
		}

		public static string SPLITWISE(string groupId, string apiKey)
		{
			return string.Format("https://home-dashboard-splitwise-mngr.herokuapp.com/group?groupId={0}&apiKey={1}", groupId, apiKey);
		}

		public static string JOURNEY_PLANNER(string start, List<string> stops, string end, string apiKey)
		{
			string url = string.Format("http://dev.virtualearth.net/REST/V1/Routes/Driving?wp.0={0}&", start);

			for (int i = 0; i < stops.Count; i++)
			{
				url += string.Format("wp.{0}={1}&", i + 1, stops[i]);
			}

			url += string.Format("wp.{0}={1}&", stops.Count + 1, end);
			url += string.Format("key={0}&distanceUnit=mi", apiKey);

			return url;
		}
	}
}