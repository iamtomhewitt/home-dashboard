using UnityEngine;
using UnityEngine.UI;

namespace BBCNews
{
	public class BBCNewsEntry : MonoBehaviour
	{
		[SerializeField] private Text title;
		[SerializeField] private Text description;

		private string url;

		public Text GetTitle()
		{
			return title;
		}

		public void SetTextColour(Color colour)
		{
			title.color = colour;
			description.color = colour;
		}

		public Text GetDescription()
		{
			return description;
		}

		public void SetUrl(string url)
		{
			this.url = url;
		}

		public void OpenInBrowser()
		{
			Application.OpenURL(url);
		}
	}
}