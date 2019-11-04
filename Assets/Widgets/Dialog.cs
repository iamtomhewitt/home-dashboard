using UnityEngine;

/// <summary>
/// A pop up dialog box on the dashboard.
/// </summary>
public abstract class Dialog : MonoBehaviour
{
	private DialogResult result;

	public DialogResult GetResult()
	{
		return result;
	}

	public void SetResult(DialogResult result)
	{
		this.result = result;
	}

	/// <summary>
	/// Has to be a reposition otherwise other components cannot find this if the Gameobject is deactivated
	/// </summary>
	public void Hide()
	{
		GetComponent<RectTransform>().localPosition = new Vector2(-100f, transform.position.y);
	}

	/// <summary>
	/// Has to be a reposition otherwise other components cannot find this if the Gameobject is deactivated
	/// </summary>
	public void Show()
	{
		GetComponent<RectTransform>().localPosition = Vector2.zero;
	}
}

public enum DialogResult
{
    YES,
    NO,
    CANCEL,
    NONE,
	FINISHED
}