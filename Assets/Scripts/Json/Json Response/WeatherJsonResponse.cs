namespace JsonResponse
{
	[System.Serializable]
	public class WeatherJsonResponse
	{
		public Currently currently;
		public Daily daily;
	}

	[System.Serializable]
	public class Currently
	{
		public string summary;
		public string icon;
		public double temperature;
	}

	[System.Serializable]
	public class Daily
	{
		public string summary;
		public Data[] data;
	}

	[System.Serializable]
	public class Data
	{
		public long time;
		public string summary;
		public string icon;
		public double temperatureHigh;
		public double temperatureLow;
	}
}