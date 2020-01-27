using UnityEngine;
using UnityEngine.UI;

namespace Dialog
{
	/// <summary>
	/// A dialog box on the dashboard.
	/// </summary>
	public abstract class Dialog : MonoBehaviour
	{
		[Header("Dialog Settings")]
		[SerializeField] private Image topBarColour;
		[SerializeField] private Text dialogTitle;
		[SerializeField] private Button hideButton;

		private DialogResult result;

		public abstract void ApplyColours();
		public abstract void ApplyAdditionalColours(Color mainColour, Color textColour);

		private void Start()
		{
			Hide();
		}

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

		public void SetCancel()
		{
			result = DialogResult.CANCEL;
		}

		public bool IsCancel()
		{
			return result == DialogResult.CANCEL;
		}

		public void SetNone()
		{
			result = DialogResult.NONE;
		}

		public bool IsNone()
		{
			return result == DialogResult.NONE;
		}

		public void SetYes()
		{
			result = DialogResult.YES;
		}

		public bool IsYes()
		{
			return result == DialogResult.YES;
		}

		public void SetNo()
		{
			result = DialogResult.NO;
		}

		public bool IsNo()
		{
			return result == DialogResult.NO;
		}

		public void SetFinished()
		{
			result = DialogResult.FINISHED;
		}

		public bool IsFinished()
		{
			return result == DialogResult.FINISHED;
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
			ApplyColours();
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