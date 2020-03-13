using UnityEngine;
using Dialog;

public class SettingsButton : MonoBehaviour
{
    public void ShowSettings()
    {
        FindObjectOfType<SettingsDialog>().Show();
    }
}
