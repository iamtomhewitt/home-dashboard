using UnityEngine;
using Dialog;

public class LogsButton : MonoBehaviour
{
	public void ShowLoggerDialog()
	{
		FindObjectOfType<WidgetLogger>().Show();
	}
}