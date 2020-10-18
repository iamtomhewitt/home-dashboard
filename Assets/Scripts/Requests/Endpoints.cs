using System.Collections.Generic;
using UnityEngine;

namespace Requests
{
	public class Endpoints : MonoBehaviour
	{
		public static Endpoints instance;

		private void Awake()
		{
			instance = this;
		}

		private string GetPlannerId()
		{
			return Config.instance.GetWidgetConfig()["foodPlanner"]["plannerId"];
		}

		public string RECIPES()
		{
			return Config.instance.GetEndpoint("recipeManager") + "/recipes";
		}

		public string PLANNER()
		{
			string plannerId = GetPlannerId();
			return Config.instance.GetEndpoint("recipeManager") + "/planner?id=" + plannerId;
		}

		public string PLANNER(string day)
		{
			string plannerId = GetPlannerId();
			string endpoint = Config.instance.GetEndpoint("recipeManager");
			return string.Format("{0}/planner/day?&id={1}&day={2}", endpoint, plannerId, day);
		}

		public string SHOPPING_LIST()
		{
			string endpoint = Config.instance.GetEndpoint("recipeManager");
			string plannerId = GetPlannerId();
			return string.Format("{0}/shoppingList?id={1}", endpoint, plannerId);
		}

		public string TRAIN_DEPARTURES(int numberOfResults)
		{
			string endpoint = Config.instance.GetEndpoint("trains");
			string stationCode = Config.instance.GetWidgetConfig()["trains"]["stationCode"];
			string apiKey = Config.instance.GetWidgetConfig()["trains"]["apiKey"];
			return string.Format(endpoint, stationCode, numberOfResults, apiKey);
		}

		public string BBC_NEWS()
		{
			string apiKey = Config.instance.GetWidgetConfig()["bbcNews"]["apiKey"];
			return string.Format(Config.instance.GetEndpoint("bbcNews"), apiKey);
		}

		public string GOOGLE_CALENDAR(string gmailAddress, int numberOfEvents, string apiKey)
		{
			string endpoint = Config.instance.GetEndpoint("googleCalendar");
			return string.Format(endpoint, gmailAddress, numberOfEvents, apiKey);
		}

		public string TODOIST_TASKS()
		{
			return Config.instance.GetEndpoint("todoist");
		}

		public string TODOIST_PROJECT(string id)
		{
			return string.Format("{0}?project_id={1}", Config.instance.GetEndpoint("todoist"), id);
		}

		public string WEATHER()
		{
			string apiKey = Config.instance.GetWidgetConfig()["weather"]["apiKey"];
			string endpoint = Config.instance.GetEndpoint("weather");
			string latitude = Config.instance.GetWidgetConfig()["weather"]["latitude"];
			string longitude = Config.instance.GetWidgetConfig()["weather"]["longitude"];
			return string.Format(endpoint, apiKey, latitude, longitude);
		}

		public string SPLITWISE()
		{
			string endpoint = Config.instance.GetEndpoint("splitwise");
			string groupId = Config.instance.GetWidgetConfig()["splitwise"]["groupId"];
			string apiKey = Config.instance.GetWidgetConfig()["splitwise"]["apiKey"];
			return string.Format(endpoint, groupId, apiKey);
		}

		public string JOURNEY_PLANNER(string start, List<string> stops, string end)
		{
			string apiKey = Config.instance.GetWidgetConfig()["journeyPlanner"]["apiKey"];
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