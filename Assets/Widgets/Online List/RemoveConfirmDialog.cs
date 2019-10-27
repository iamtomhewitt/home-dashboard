using UnityEngine;
using UnityEngine.UI;

namespace OnlineLists
{
	public class RemoveConfirmDialog : MonoBehaviour
	{
		public Text infoText;
		private DialogResult result;

		private void Start()
		{
			Hide();
		}

		/// <summary>
		/// Has to be a reposition otherwise other components cannot find this if the Gameobject is deactivated
		/// </summary>
		public void Hide()
		{
			GetComponent<RectTransform>().localPosition = new Vector2(-100f, transform.position.y);
		}

		/// <summary>
		/// Has to be a reposition otherwise other components cannot find this if the Gameobject is deactivated
		/// </summary>
		public void Show(string message)
		{
			GetComponent<RectTransform>().localPosition = Vector2.zero;
			infoText.text = message;
		}

		public void SetNone()
		{
			result = DialogResult.NONE;
		}

		public DialogResult GetResult()
		{
			return result;
		}

		public void Yes()
		{
			this.result = DialogResult.YES;
		}

		public void No()
		{
			this.result = DialogResult.NO;
		}

		public void Cancel()
		{
			this.result = DialogResult.CANCEL;
		}

		public enum DialogResult
		{
			YES,
			NO,
			CANCEL,
			NONE
		}
	}
}
