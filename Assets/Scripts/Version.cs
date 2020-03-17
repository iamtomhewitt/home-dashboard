using UnityEngine;
using UnityEngine.UI;
using Dialog;
using TMPro;

/// <summary>
/// Shows the application version in the bottom left of the dashboard.
/// </summary>
public class Version : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Button>().GetComponentInChildren<TMP_Text>().text = "Version: " + Application.version;
    }

    public void ShowLog()
    {
        FindObjectOfType<WidgetLogger>().Show();
    }
}
