using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Dialog;
using SimpleJSON;
using OnlineLists;
using Requests;

namespace Planner
{
	public class FoodPlanner : Widget
	{
		[Header("Food Planner Settings")]
		[SerializeField] private OnlineList shoppingList;
		[SerializeField] private Image addButton;

		public override void Start()
		{
			base.Start();

			JSONNode config = Config.instance.GetWidgetConfig()[GetWidgetConfigKey()];

			addButton.color = Colours.Darken(GetWidgetColour());

			foreach (PlannerEntry planner in FindObjectsOfType<PlannerEntry>())
			{
				planner.SetDayTextColour(GetTextColour());
				planner.SetRecipeTextColour(Colours.ToColour(config["plannerTextColour"]));
				planner.SetRecipeBackgroundColour(Colours.ToColour(config["plannerBackgroundColour"]));
				planner.SetDayBackgroundColour(Colours.Lighten(GetWidgetColour()));
			}
		}

		public override void ReloadConfig() {}

		public override void Run() {}

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
				dialog.Hide();

				List<Ingredient> ingredients = new List<Ingredient>();

				// For each day
				PlannerEntry[] plannerEntries = FindObjectsOfType<PlannerEntry>();
				foreach (PlannerEntry entry in plannerEntries)
				{
					JSONObject body = new JSONObject();
					body.Add("apiKey", Config.instance.GetWidgetConfig()[GetWidgetConfigKey()]["apiKey"]);

					// Get the ingredients by recipe
					UnityWebRequest request = Postman.CreateGetRequest(Endpoints.RECIPES + "?name=" + entry.GetRecipeName(), body);
					yield return request.SendWebRequest();

					JSONNode json = JSON.Parse(request.downloadHandler.text);
					
					if (json["status"] == 400)
					{
						// A free text recipe may have been entered, so there will be no ingredients to add, therefore just move onto the next recipe
						continue;
					}

					for (int i = 0; i < json["recipe"]["ingredients"].AsArray.Count; i++)
					{
						JSONNode node = json["recipe"]["ingredients"][i];

						Ingredient ingredient = new Ingredient(node["name"], node["category"], node["weight"], node["amount"]);
						Ingredient existingIngredient = ingredients.Find(x => x.name.Equals(ingredient.name) && x.weight.Equals(ingredient.weight));

						// Update the existing ingredient if it exists
						if (existingIngredient != null)
						{
							double amount = existingIngredient.amount;
							double amountToAdd = node["amount"];
							existingIngredient.amount = amount + amountToAdd;
						}
						else
						{
							ingredients.Add(ingredient);
						}
					}
				}

				foreach (Ingredient ingredient in ingredients)
				{
					yield return new WaitForSeconds(0.1f);
					shoppingList.AddItem(ingredient.name + " (" + ingredient.amount + " " + ingredient.weight + ")");
				}

				dialog.SetNone();
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