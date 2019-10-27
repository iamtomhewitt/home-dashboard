using UnityEngine;
using UnityEngine.UI;

namespace BBCNews
{
	public class BBCNewsEntry : MonoBehaviour
	{
		public Text title;
		public Text description;

		[HideInInspector] public string url;

		public void OpenInBrowser()
		{
			Application.OpenURL(url);
		}
	}
}