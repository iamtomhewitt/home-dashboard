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

        public static string GOOGLE_CALENDAR(string gmailAddress, string future, string today, string apiKey)
        {
            return string.Format("https://www.googleapis.com/calendar/v3/calendars/{0}/events?orderBy=startTime&singleEvents=true&timeMax={1}T10:00:00-07:00&timeMin={2}T10:00:00-07:00&key={3}", gmailAddress, future, today, apiKey);
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

        public static string JOURNEY_PLANNER(string start, string end, string apiKey)
        {
            return string.Format("http://dev.virtualearth.net/REST/V1/Routes/Driving?wp.0={0}&wp.1={1}&key={2}&distanceUnit=mi", start, end, apiKey);
        }
    }
}