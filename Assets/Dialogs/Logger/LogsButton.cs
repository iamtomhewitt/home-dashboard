using Dialog;
using UnityEngine;

public class LogsButton : MonoBehaviour
{
	public void ShowLoggerDialog()
	{
		FindObjectOfType<WidgetLogger>().Show();
	}
}