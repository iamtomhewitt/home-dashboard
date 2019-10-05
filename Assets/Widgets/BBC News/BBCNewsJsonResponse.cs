[System.Serializable]
public class BBCNewsJsonResponse
{
	public Article[] articles;
}

[System.Serializable]
public class Article
{
	public string title;
	public string description;
	public string url;
}
