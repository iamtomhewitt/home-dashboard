using SimpleJSON;
using UnityEngine;

namespace Dialog
{
	/// <summary>
	/// An abstract dialog that can be shown from anywhere and is not based on a widget.
	/// </summary>
	public abstract class PopupDialog : Dialog
	{
		[Header("Popup Dialog Settings")]
		[SerializeField] private string configKey;

		public override void ApplyColours()
		{
			JSONNode config = Config.instance.GetDialogConfig()[configKey];

			Color mainColour = Colours.ToColour(config["mainColour"]);
			Color textColour = Colours.ToColour(config["textColour"]);

			SetDialogTitleColour(textColour);
			SetTopBarColour(mainColour);
			SetHideButtonColour(mainColour, textColour);

			ApplyAdditionalColours(mainColour, textColour);
		}

		public string GetConfigKey()
		{
			return configKey;
		}
	}
}