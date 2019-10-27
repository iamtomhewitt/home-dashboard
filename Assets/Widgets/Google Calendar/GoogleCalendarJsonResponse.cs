namespace GoogleCalendar
{
	[System.Serializable]
	public class GoogleCalendarJsonResponse
	{
		public Item[] items;
	}

	[System.Serializable]
	public class Item
	{
		public string summary;
		public Start start;
	}

	[System.Serializable]
	public class Start
	{
		public string date;
		public string dateTime;
	}
}