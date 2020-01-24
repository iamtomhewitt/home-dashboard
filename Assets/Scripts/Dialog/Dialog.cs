using UnityEngine;
using UnityEngine.UI;

namespace Dialog
{
	/// <summary>
	/// A pop up dialog box on the dashboard.
	/// </summary>
	public abstract class Dialog : MonoBehaviour
	{
		[Header("Dialog Settings")]
		[SerializeField] private Image topBarColour;
		[SerializeField] private Text dialogTitle;
		[SerializeField] private Button hideButton;

		private DialogResult result;

		public void SetTopBarColour(Color colour)
		{
			topBarColour.color = colour;
		}

		public void SetDialogTitleColour(Color colour)
		{
			dialogTitle.color = colour;
		}

		public void SetHideButtonColour(Color mainColour, Color textColour)
		{
			hideButton.GetComponent<Image>().color = mainColour;
			hideButton.GetComponentInChildren<Text>().color = textColour;
		}

		public DialogResult GetResult()
		{
			return result;
		}

		public void SetResult(DialogResult result)
		{
			this.result = result;
		}

		public void Cancel()
		{
			result = DialogResult.CANCEL;
		}

		public void None()
		{
			result = DialogResult.NONE;
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
		public void Show()
		{
			GetComponent<RectTransform>().localPosition = Vector2.zero;
		}
	}

	public enum DialogResult
	{
		YES,
		NO,
		CANCEL,
		NONE,
		FINISHED
	}
}