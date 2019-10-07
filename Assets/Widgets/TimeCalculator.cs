using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TimeCalculator : MonoBehaviour
{	
	public static float ToSeconds(TimeUnit unit, float amount)
	{
		float seconds = 0f;

		switch (unit)
		{
			case TimeUnit.Seconds:
				seconds = amount;
				break;

			case TimeUnit.Minutes:
				seconds = amount * 60f;
				break;

			case TimeUnit.Hours:
				seconds = amount * 3600f;
				break;

			case TimeUnit.Days:
				seconds = amount * 86400f;
				break;

			default:
				print("Unknown unit: " + unit);
				seconds = 600f;
				break;
		}

		print(amount + " " + unit + ": " + seconds + " seconds");
		
		return seconds;
	}
}

[System.Serializable]
public enum TimeUnit
{
	Seconds,
	Minutes,
	Hours,
	Days
}
