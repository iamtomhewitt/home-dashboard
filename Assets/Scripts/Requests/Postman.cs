using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using SimpleJSON;

namespace Requests
{
	/// <summary>
	/// Static class for making http requests with JSON payloads.
	/// </summary>
	public class Postman : MonoBehaviour
	{
		public static UnityWebRequest CreateGetRequest(string url)
		{
			return UnityWebRequest.Get(url);
		}

		public static UnityWebRequest CreatePostRequest(string url, JSONObject body)
		{
			return PostRequest(url, body.ToString());
		}

		public static UnityWebRequest CreatePostRequest(string url, string body)
		{
			return PostRequest(url, body);
		}

		public static UnityWebRequest CreatePutRequest(string url, string body)
		{
			return PutRequest(url, body);
		}

		private static UnityWebRequest PostRequest(string url, string jsonBodyAsString)
		{
			UnityWebRequest request = UnityWebRequest.PostWwwForm(url, "POST");
			byte[] bytes = Encoding.UTF8.GetBytes(jsonBodyAsString);

			request.uploadHandler = new UploadHandlerRaw(bytes);
			request.downloadHandler = new DownloadHandlerBuffer();
			request.SetRequestHeader("Content-Type", "application/json");

			return request;
		}

		private static UnityWebRequest PutRequest(string url, string jsonBodyAsString)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(jsonBodyAsString);
			UnityWebRequest request = UnityWebRequest.Put(url, bytes);
			request.SetRequestHeader("Content-Type", "application/json");

			return request;
		}
	}
}