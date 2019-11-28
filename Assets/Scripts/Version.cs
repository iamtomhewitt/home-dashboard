using Dialog;
using UnityEngine;
using UnityEngine.UI;

public class Version : MonoBehaviour
{
    private void Start()
    {
		Button button = GetComponent<Button>();
		button.GetComponentInChildren<Text>().text = "Version: " + Application.version;
    }
	
	/// <summary>
	/// Called from a button.
	/// </summary>
	public void ShowLog()
	{
		FindObjectOfType<WidgetLogger>().Show();
	}
}
