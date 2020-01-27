using UnityEngine;
using SimpleJSON;

namespace Dialog
{
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