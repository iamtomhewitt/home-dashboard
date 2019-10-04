using UnityEngine;
using UnityEngine.UI;

public class ListItem : MonoBehaviour
{
	public Text itemName;

	public void SetName(string name)
	{
		this.itemName.text = name;
	}

	public void Delete()
	{
		Destroy(this.gameObject);
	}
}
