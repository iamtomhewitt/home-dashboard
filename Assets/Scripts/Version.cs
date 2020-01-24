using UnityEngine;
using UnityEngine.UI;

public class Version : MonoBehaviour
{
    private void Start()
    {
		Button button = GetComponent<Button>();
		button.GetComponentInChildren<Text>().text = "Version: " + Application.version;
    }
}
