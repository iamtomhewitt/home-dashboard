using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using SimpleJSON;

/// <summary>
/// Static class for making http requests with JSON payloads.
/// </summary>
public class Postman : MonoBehaviour
{
	public static UnityWebRequest CreateGetRequest(string url)
	{
		return UnityWebRequest.Get(url);
	}

	public static UnityWebRequest CreatePostRequest(string url, JSONObject json)
	{
		UnityWebRequest request = UnityWebRequest.Post(url, "POST");
		byte[] body = Encoding.UTF8.GetBytes(json.ToString());

		request.uploadHandler = new UploadHandlerRaw(body);
		request.downloadHandler = new DownloadHandlerBuffer();
		request.SetRequestHeader("Content-Type", "application/json");

		return request;
	}
}
