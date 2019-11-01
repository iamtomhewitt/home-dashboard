using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A pop up dialog box on the dashboard.
/// </summary>
public abstract class Dialog : MonoBehaviour
{
    public abstract void Show();
    public abstract void Hide();

    // [SerializeField] private Color dialogColour;
    // [SerializeField] private Text titleText;
    // [SerializeField] private Image windowBar;

    // public void SetWindowColour(Color color)
    // {
    //     windowBar.color = color;
    // }

    // public void SetTitleText(string text)
    // {
    //     titleText.text = text;
    // }
}

public enum DialogResult
{
    YES,
    NO,
    CANCEL,
    NONE
}