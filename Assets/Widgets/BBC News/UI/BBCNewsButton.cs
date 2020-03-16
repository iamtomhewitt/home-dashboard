using UnityEngine;

namespace BBCNews
{
	public class BBCNewsButton : MonoBehaviour
	{
		private void Start()
		{
			// Align the button to the center of the widget
			transform.position = FindObjectOfType<BBCNews>().transform.position;
		}

		/// <summary>
		/// Called from a button.
		/// </summary>
		public void OpenArticle()
		{
			FindObjectOfType<BBCNewsEntry>().OpenInBrowser();
		}
	}
}