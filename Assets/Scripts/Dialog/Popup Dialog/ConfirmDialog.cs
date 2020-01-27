using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;

namespace Dialog
{
	/// <summary>
	/// A Yes/No dialog that can be used to check if an action can be proceeded with.
	/// </summary>
	public class ConfirmDialog : PopupDialog
	{
		[Header("Confirm Settings")]
		[SerializeField] private Text infoText;
		[SerializeField] private Button yesButton;
		[SerializeField] private Button noButton;

		public override void ApplyAdditionalColours(Color mainColour, Color textColour)
		{
			yesButton.GetComponent<Image>().color = mainColour;
			yesButton.GetComponentInChildren<Text>().color = textColour;

			noButton.GetComponent<Image>().color = mainColour;
			noButton.GetComponentInChildren<Text>().color = textColour;
		}

		public void SetInfoMessage(string message)
		{
			infoText.text = message;
		}
	}
}