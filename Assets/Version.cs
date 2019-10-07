using UnityEngine;
using UnityEngine.UI;

public class Version : MonoBehaviour
{
    private void Start()
    {
		GetComponent<Text>().text = "Version: " + Application.version;
    }
}
