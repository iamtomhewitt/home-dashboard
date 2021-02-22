using TMPro;
using UnityEngine.UI;
using UnityEngine;

namespace Dialog
{
	/// <summary>
	/// A Yes/No dialog that can be used to check if an action can be proceeded with.
	/// </summary>
	public class ConfirmDialog : PopupDialog
	{
		[Header("Confirm Settings")]
		[SerializeField] private Button noButton;
		[SerializeField] private Button yesButton;
		[SerializeField] private TMP_Text infoText;

		public override void ApplyAdditionalColours(Color mainColour, Color textColour)
		{
			yesButton.GetComponent<Image>().color = mainColour;
			yesButton.GetComponentInChildren<TMP_Text>().color = textColour;

			noButton.GetComponent<Image>().color = mainColour;
			noButton.GetComponentInChildren<TMP_Text>().color = textColour;
		}

		public void SetInfoMessage(string message)
		{
			infoText.text = message;
		}

		public override void PostShow()
		{
			// Nothing to do
		}
	}
}