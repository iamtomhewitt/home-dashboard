using UnityEngine;
using System.Collections.Generic;

namespace Requests
{
	public class Endpoints : MonoBehaviour
	{
		public static Endpoints instance;

		private void Awake()
		{
			instance = this;
		}

		public string RECIPES_ADD()
		{
			return Config.instance.GetEndpoint("recipeManager") + "/recipes/add";
		}

		public string PLANNER_ADD()
		{
			return Config.instance.GetEndpoint("recipeManager") + "/planner/add";
		}

		public string PLANNER(string day)
		{
			string plannerId = Config.instance.GetWidgetConfig()["foodPlanner"]["plannerId"];
			string apiKey = Config.instance.GetWidgetConfig()["foodPlanner"]["apiKey"];
			string endpoint = Config.instance.GetEndpoint("recipeManager");
			string s = string.Format("{0}/planner?apiKey={1}&plannerId={2}", endpoint, apiKey, plannerId);
			if (!string.IsNullOrEmpty(day))
			{
				s += string.Format("&day={0}", day);
			}
			return s;
		}

		public string RECIPES(string apiKey)
		{
			return string.Format("{0}/recipes?apiKey={1}", Config.instance.GetEndpoint("recipeManager"), apiKey);
		}

		public string SHOPPING_LIST()
		{
			string endpoint = Config.instance.GetEndpoint("recipeManager");
			string plannerId = Config.instance.GetWidgetConfig()["foodPlanner"]["plannerId"];
			string apiKey = Config.instance.GetWidgetConfig()["foodPlanner"]["apiKey"];
			return string.Format("{0}/shoppingList?plannerId={1}&apiKey={2}", endpoint, plannerId, apiKey);
		}

		public string TRAIN_DEPARTURES(string stationCode, int numberOfResults, string apiKey)
		{
			return string.Format(Config.instance.GetEndpoint("trains"), stationCode, numberOfResults, apiKey);
		}

		public string BBC_NEWS()
		{
			string apiKey = Config.instance.GetWidgetConfig()["bbcNews"]["apiKey"];
			return string.Format(Config.instance.GetEndpoint("bbcNews"), apiKey);
		}

		public string GOOGLE_CALENDAR(string apiKey, string gmailAddress, int numberOfEvents)
		{
			return string.Format(Config.instance.GetEndpoint("googleCalendar"), apiKey, gmailAddress, numberOfEvents);
		}

		public string TODOIST_TASKS()
		{
			return Config.instance.GetEndpoint("todoist");
		}

		public string TODOIST_PROJECT(string id)
		{
			return string.Format("{0}?project_id={1}", Config.instance.GetEndpoint("todoist"), id);
		}

		public string WEATHER(string apiKey, string latitude, string longitude)
		{
			return string.Format(Config.instance.GetEndpoint("weather"), apiKey, latitude, longitude);
		}

		public string SPLITWISE(string groupId, string apiKey)
		{
			return string.Format(Config.instance.GetEndpoint("splitwise"), groupId, apiKey);
		}

		public string JOURNEY_PLANNER(string start, List<string> stops, string end, string apiKey)
		{
			string url = string.Format(Config.instance.GetEndpoint("journeyPlanner"), start);

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