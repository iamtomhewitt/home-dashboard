using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class GeneralSettings : MonoBehaviour
{
	[SerializeField] private Camera mainCamera;
	[SerializeField] private Image settingsButton;
	[SerializeField] private TMP_Text logsButton;
	[SerializeField] private TMP_Text versionButton;

	public void Start()
	{
		string backgroundColor = Config.instance.GetGeneralConfig()["backgroundColour"];
		string buttonColour = Config.instance.GetGeneralConfig()["buttonColour"];

		if (backgroundColor != null)
		{
			mainCamera.backgroundColor = Colours.ToColour(backgroundColor);
		}

		if (buttonColour != null)
		{
			settingsButton.color = Colours.ToColour(buttonColour);
			logsButton.color = Colours.ToColour(buttonColour);
			versionButton.color = Colours.ToColour(buttonColour);
		}
	}
}
