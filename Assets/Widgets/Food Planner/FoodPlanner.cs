using Dialog;
using OnlineLists;
using Requests;
using SimpleJSON;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine;

namespace Planner
{
	public class FoodPlanner : Widget
	{
		[Header("Food Planner Settings")]
		[SerializeField] private Button addButton;

		public override void Start()
		{
			base.Start();

			JSONNode config = Config.instance.GetWidgetConfig()[GetWidgetConfigKey()];

			addButton.GetComponent<Image>().color = Colours.Darken(GetWidgetColour());

			foreach (PlannerEntry planner in FindObjectsOfType<PlannerEntry>())
			{
				planner.SetDayTextColour(GetTextColour());
				planner.SetRecipeTextColour(Colours.ToColour(config["plannerTextColour"]));
				planner.SetRecipeBackgroundColour(Colours.ToColour(config["plannerBackgroundColour"]));
				planner.SetDayBackgroundColour(Colours.Lighten(GetWidgetColour()));
			}
		}

		public override void ReloadConfig() { }

		public override void Run() { }

		/// <summary>
		/// Adds all the ingredients from all the recipes to the shopping list.
		/// </summary>
		public void AddToShoppingList()
		{
			StartCoroutine(AddToShoppingListRoutine());
		}

		private IEnumerator AddToShoppingListRoutine()
		{
			ConfirmDialog dialog = FindObjectOfType<ConfirmDialog>();
			dialog.ApplyColours();
			dialog.Show();
			dialog.SetNone();
			dialog.SetInfoMessage("Add all ingredients from each recipe to the shopping list?");

			while (dialog.IsNone())
			{
				yield return null;
			}

			if (dialog.IsNo())
			{
				dialog.Hide();
				dialog.SetNone();
				yield break;
			}

			if (dialog.IsYes())
			{
				TMP_Text addButtonText = addButton.GetComponentInChildren<TMP_Text>();
				addButtonText.text = "Please wait...";
				addButton.interactable = false;

				dialog.Hide();

				OnlineList shoppingList = FindObjectsOfType<OnlineList>().Where(x => x.GetListType().Equals(TodoistList.shoppingList)).First();
				UnityWebRequest request = Postman.CreateGetRequest(Endpoints.instance.SHOPPING_LIST());
				yield return request.SendWebRequest();

				JSONArray json = JSON.Parse(request.downloadHandler.text).AsArray;
				foreach (KeyValuePair<string, JSONNode> a in json)
				{
					string item = a.Value.Value.Replace("quantity of ", "");
					yield return StartCoroutine(shoppingList.AddItemRoutine(item));
				}

				shoppingList.Refresh();
				dialog.SetNone();

				addButton.interactable = true;
				addButtonText.text = "Add To Shopping List";

				yield break;
			}
		}
	}

	public class Ingredient
	{
		public string name;
		public string category;
		public string weight;
		public double amount;

		public Ingredient(string name, string category, string weight, double amount)
		{
			this.name = name;
			this.category = category;
			this.weight = weight;
			this.amount = amount;
		}
	}
}