using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
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
		LoadItemsFromFile();
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
		statusText.text = "";
	}

	// Called from the add button on the menu that pops up
	public void AddItem(InputField input)
	{
		AddItem(input.text);
		input.text = "";
	}

	public void AddItem(string text)
	{
		GameObject item = Instantiate(itemPrefab, itemParent) as GameObject;
		item.GetComponent<ListItem>().SetName(text);
		statusText.text = "'" + text + "' added!";

		this.SaveToFile();
		this.UpdateLastUpdatedText();
	}

	public void SaveToFile()
	{
		// Find the items
		ListItem[] currentItems = FindObjectsOfType<ListItem>();

		string saveString = "";

		foreach (ListItem item in currentItems)
		{
			saveString += item.itemName.text + ",";
		}
		saveString = saveString.Remove(saveString.Length - 1);

		string path = Application.persistentDataPath + "/items.txt";

		// Delete the old file as we are going to create a new one
		File.Delete(path);

		// Write the contents
		StreamWriter writer = new StreamWriter(path, true);
		writer.WriteLine(saveString);
		writer.Close();
	}

	private void LoadItemsFromFile()
	{
		string path = Application.persistentDataPath + "/items.txt";
		StreamReader reader;

		try
		{
			reader = new StreamReader(path);
		}
		catch (FileNotFoundException e)
		{
			// File does not exist, create
			File.Create(path);
			return;
		}

		string itemList = "";

		while (!reader.EndOfStream)
		{
			itemList += reader.ReadLine();
		}
		reader.Close();

		string[] items = itemList.Split(',');
		foreach (string item in items)
		{
			if (!item.Equals(""))
			{
				AddItem(item);
			}
		}
	}
}

