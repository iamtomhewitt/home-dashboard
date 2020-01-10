using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using Dialog;
using Dialog.OnlineLists;
using SimpleJSON;

namespace FoodPlannerWidget
{
	public class FoodPlanner : Widget
	{
		[Header("Food Planner Settings")]
		[SerializeField] private AddItemDialog shoppingList;

		private void Start()
		{
			this.Initialise();
			InvokeRepeating("Run", 0f, RepeatRateInSeconds());
		}

		public override void Run()
		{
			// Nothing to do!
		}

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
			dialog.Show();
			dialog.None();
			dialog.SetInfoMessage("Add all ingredients from each recipe to the shopping list?");

			while (dialog.GetResult() == DialogResult.NONE)
			{
				yield return null;
			}

			if (dialog.GetResult() == DialogResult.NO)
			{
				dialog.Hide();
				dialog.None();
				yield break;
			}

			if (dialog.GetResult() == DialogResult.YES)
			{
				List<Ingredient> ingredients = new List<Ingredient>();

				// For each day
				PlannerEntry[] plannerEntries = FindObjectsOfType<PlannerEntry>();
				foreach (PlannerEntry entry in plannerEntries)
				{
					// Get the ingredients by recipe
					UnityWebRequest request = Postman.CreateGetRequest(RecipeManagerEndpoints.RECIPES + "?name=" + entry.GetRecipeName());
					yield return request.SendWebRequest();

					JSONNode responseJson = JSON.Parse(request.downloadHandler.text);
					
					if (responseJson["status"] == 404)
					{
						// A free text recipe may have been entered, so there will be no ingredients to add, therefore just move onto the next recipe
						continue;
					}

					for (int i = 0; i < responseJson["recipe"]["ingredients"].AsArray.Count; i++)
					{
						JSONNode node = responseJson["recipe"]["ingredients"][i];
						Ingredient ingredient = new Ingredient(node["name"], node["category"], node["weight"], node["amount"]);

						// Update the existing ingredient if it exists
						Ingredient existingIngredient = ingredients.Find(x => x.name.Equals(ingredient.name) && x.weight.Equals(ingredient.weight));

						if (existingIngredient != null)
						{
							int amount = existingIngredient.amount;
							int amountToAdd = node["amount"];
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
					shoppingList.AddItem(ingredient.name + " (" + ingredient.amount + " " + ingredient.weight + ")");
				}

				dialog.Hide();
				dialog.None();
				yield break;
			}
		}
	}

	public class Ingredient
	{
		public string name;
		public string category;
		public string weight;
		public int amount;

		public Ingredient(string name, string category, string weight, int amount)
		{
			this.name = name;
			this.category = category;
			this.weight = weight;
			this.amount = amount;
		}
	}
}