using TMPro;
using UnityEngine;

namespace BBCNews
{
	public class BBCNewsEntry : MonoBehaviour
	{
		[SerializeField] private TMP_Text title;
		[SerializeField] private TMP_Text description;

		private string url;

		public void SetTitle(string t)
		{
			title.text = t;
		}

		public void SetTextColour(Color colour)
		{
			title.color = colour;
			description.color = colour;
		}

		public void SetDescription(string desc)
		{
			description.text = desc;
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