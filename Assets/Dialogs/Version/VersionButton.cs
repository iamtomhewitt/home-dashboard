using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Shows the application version in the bottom left of the dashboard.
/// </summary>
public class VersionButton : MonoBehaviour
{
	public void SetVersionText(string text)
	{
		GetComponent<Button>().GetComponentInChildren<TMP_Text>().text = text;
	}

	public void ShowVersionDialog()
	{
		FindObjectOfType<VersionDialog>().Show();
	}
}