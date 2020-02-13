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

		public static UnityWebRequest CreateGetRequest(string url, JSONObject body)
		{
			UnityWebRequest request = UnityWebRequest.Get(url);
			byte[] bytes = Encoding.UTF8.GetBytes(body.ToString());

			request.uploadHandler = new UploadHandlerRaw(bytes);
			request.downloadHandler = new DownloadHandlerBuffer();
			request.SetRequestHeader("Content-Type", "application/json");

			return request;
		}

		public static UnityWebRequest CreatePostRequest(string url, JSONObject body)
		{
			UnityWebRequest request = UnityWebRequest.Post(url, "POST");
			byte[] bytes = Encoding.UTF8.GetBytes(body.ToString());

			request.uploadHandler = new UploadHandlerRaw(bytes);
			request.downloadHandler = new DownloadHandlerBuffer();
			request.SetRequestHeader("Content-Type", "application/json");

			return request;
		}

		public static UnityWebRequest CreateTodoistRequest(string url, string json, string apiKey, string uuid)
		{
			UnityWebRequest request = new UnityWebRequest(url, "POST");
			byte[] jsonToSend = new UTF8Encoding().GetBytes(json);

			request.uploadHandler = new UploadHandlerRaw(jsonToSend);
			request.downloadHandler = new DownloadHandlerBuffer();

			request.SetRequestHeader("Content-Type", "application/json");
			request.SetRequestHeader("Authorization", "Bearer " + apiKey);
			request.SetRequestHeader("X-Request-Id", uuid);

			return request;
		}
	}
}