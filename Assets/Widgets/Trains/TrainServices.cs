using UnityEngine;

[System.Serializable]
public class TrainJsonResponse
{
	public TrainServices[] trainServices;	
}

[System.Serializable]
public class TrainServices
{
	public Destination[] destination;
	public string std;
	public string etd;
}

[System.Serializable]
public class Destination
{
	public string locationName;
}
