using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A key lavel and value input field on the settings page.
/// </summary>
public class Setting : MonoBehaviour
{
	[SerializeField] private string[] keyTree;
	[SerializeField] private InputField value;

	/// <summary>
	/// Returns the key of the value of the tree.
	/// E.g. apiKeys->googleCalendars->someone@gmail.com would return the key someone@gmail.com
	/// </summary>
	/// <returns></returns>
	public string[] GetKeyTree()
	{
		return keyTree;
	}

	public string GetValue()
	{
		return value.text;
	}
}