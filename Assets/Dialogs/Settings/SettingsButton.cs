using Dialog;
using UnityEngine;

public class SettingsButton : MonoBehaviour
{
    public void ShowSettings()
    {
        FindObjectOfType<SettingsDialog>().Show();
    }
}