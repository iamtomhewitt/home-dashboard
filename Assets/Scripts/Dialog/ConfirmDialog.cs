using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;

namespace Dialog
{
	public class ConfirmDialog : Dialog
	{
		[Header("Confirm Settings")]
		[SerializeField] private Text infoText;
		[SerializeField] private Button yesButton;
		[SerializeField] private Button noButton;

		private void Start()
		{
			Hide();
		}

		public void ApplyColours()
		{
			JSONNode config = Config.instance.GetDialogConfig()["confirm"];
			Color mainColour = Utils.ToColour(config["mainColour"]);
			Color textColour = Utils.ToColour(config["textColour"]);

			SetDialogTitleColour(textColour);
			SetTopBarColour(mainColour);
			SetHideButtonColour(mainColour, textColour);

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