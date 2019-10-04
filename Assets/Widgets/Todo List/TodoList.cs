using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TodoList : Widget
{
	[Space(15f)]
	public GameObject addItemMenu;
	public GameObject itemPrefab;
	public Transform itemParent;
	public Text statusText;

	private void Start()
	{
		this.Initialise();
		HideMenu();
	}

	public override void Run()
	{
		// Currently do nothing
	}

	// Called from a button
	public void ShowMenu()
	{
		addItemMenu.SetActive(true);
	}

	// Called from a button
	public void HideMenu()
	{
		addItemMenu.SetActive(false);
	}

	// Called from the add button on the menu that pops up
	public void AddItem(InputField input)
	{
		GameObject item = Instantiate(itemPrefab, itemParent) as GameObject;
		item.GetComponent<ListItem>().SetName(input.text);
		statusText.text = "'" + input.text + "' added!";
		input.text = "";
		this.UpdateLastUpdatedText();
	}
}

