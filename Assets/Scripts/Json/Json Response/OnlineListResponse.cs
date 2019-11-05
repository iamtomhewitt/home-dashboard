namespace JsonResponse
{
	[System.Serializable]
	public class OnlineListResponse
	{
		public OnlineListData leaderboard;
	}

	[System.Serializable]
	public class OnlineListData
	{
		public OnlineListItem[] entry;
	}

	[System.Serializable]
	public class OnlineListItem
	{
		public string name;
	}

	[System.Serializable]
	public class OnlineListSingleResponse
	{
		public OnlineListSingleData leaderboard;
	}

	[System.Serializable]
	public class OnlineListSingleData
	{
		public OnlineListItem entry;
	}
}