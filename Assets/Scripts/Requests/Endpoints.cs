using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Requests
{
	public class Endpoints : MonoBehaviour
	{
		public static string TRAIN_DEPARTURES(string stationCode, int numberOfResults, string apiKey)
		{
			return "https://home-dashboard-train-manager.herokuapp.com/departures?stationCode=" + stationCode + "&numberOfResults=" + numberOfResults + "&apiKey=" + apiKey;
		}
	}
}