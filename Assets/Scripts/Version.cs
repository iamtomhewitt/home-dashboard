using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Shows the application version in the bottom left of the dashboard.
/// </summary>
public class Version : MonoBehaviour
{
    private void Start()
    {
		GetComponent<Button>().GetComponentInChildren<Text>().text = "Version: " + Application.version;
    }
}
