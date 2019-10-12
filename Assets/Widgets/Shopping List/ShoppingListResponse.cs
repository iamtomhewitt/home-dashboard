[System.Serializable]
public class ShoppingListResponse
{
	public ShoppingListData leaderboard;
}

[System.Serializable]
public class ShoppingListData
{
	public ShoppingListItem[] entry;
}

[System.Serializable]
public class ShoppingListItem
{
	public string name;
}
