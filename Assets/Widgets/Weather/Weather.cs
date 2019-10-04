using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Weather : Widget
{
	[Space(15f)]
	public string apiKey;
	public string latitude;
	public string longitude;

	public Text currentSummary;
	public Image currentIcon;
	public Text currentTemperature;

	public WeatherEntry[] entries;
	public Sprite[] weatherSprites;

	private void Start()
	{
		this.Initialise();
		InvokeRepeating("Run", 0f, this.repeatRate);
	}

	public override void Run()
	{
		StartCoroutine(RunRoutine());
		this.UpdateLastUpdatedText();
	}

	private IEnumerator RunRoutine()
	{
		string url = "https://api.darksky.net/forecast/" + apiKey + "/" + latitude + "," + longitude + "?units=uk";

		UnityWebRequest request = UnityWebRequest.Get(url);
		yield return request.SendWebRequest();
		string jsonResponse = request.downloadHandler.text;

		WeatherJsonResponse response = JsonUtility.FromJson<WeatherJsonResponse>(jsonResponse);

		currentSummary.text = response.currently.summary;
		currentIcon.sprite = GetSpriteForName(response.currently.icon);
		currentTemperature.text = Mathf.RoundToInt((float)response.currently.temperature).ToString() + "°";

		for (int i = 0; i < entries.Length; i++)
		{
			Data day = response.daily.data[i + 1];
			WeatherEntry entry = entries[i];

			DateTime date = new DateTime(1970, 1, 1, 0, 0, 0, 0);
			date = date.AddSeconds(day.time);

			entry.day.text = date.DayOfWeek.ToString();
			entry.icon.sprite = GetSpriteForName(day.icon);
			entry.temperatureHigh.text = Mathf.RoundToInt((float)day.temperatureHigh).ToString() + "°";
			entry.temperatureLow.text = Mathf.RoundToInt((float)day.temperatureLow).ToString() + "°";
		}
	}

	private Sprite GetSpriteForName(string weatherName)
	{
		foreach (Sprite weatherSprite in weatherSprites)
		{
			if (weatherSprite.name == weatherName)
			{
				return weatherSprite;
			}
		}

		print("Could not find: " + weatherName);
		return null;
	}
}
