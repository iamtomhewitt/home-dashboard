using UnityEngine;
using System.IO;
using JsonHelper;
using Dialog;
using System.Collections;
using System.Collections.Generic;
using static Recipe;
using System.Linq;
using OnlineLists;

namespace FoodPlanner
{
	public class FoodPlanner : Widget
	{
		[SerializeField] private RecipeCard[] recipeCards;
		[SerializeField] private OnlineList shoppingList;
		[SerializeField] private string filePath;

		private void Start()
		{
			filePath = Path.Combine(Application.persistentDataPath, "foodplanner.json");
			recipeCards = FindObjectsOfType<RecipeCard>();

			LoadRecipesFromFile();

			this.Initialise();
			InvokeRepeating("Run", 0f, RepeatRateInSeconds());
		}

		public override void Run()
		{
			// Nothing to do!
		}

		/// <summary>
		/// Saves the recipes to file.
		/// </summary>
		public void SaveToFile()
		{
			RecipeData[] data = new RecipeData[recipeCards.Length];

			for (int i = 0; i < recipeCards.Length; i++)
			{
				data[i] = recipeCards[i].GetRecipeData();
			}

			string json = FoodPlannerJsonHelper.ToJson(data);

			StreamWriter writer = new StreamWriter(filePath, false);
			writer.WriteLine(json);
			writer.Close();
		}

		/// <summary>
		/// Loads the recipes from files
		/// </summary>
		private void LoadRecipesFromFile()
		{
			if (!File.Exists(filePath))
			{
				SaveToFile();
			}

			string fileContent = File.ReadAllText(filePath);

			RecipeData[] data = FoodPlannerJsonHelper.FromJson(fileContent);

			for (int i = 0; i < data.Length; i++)
			{
				//print(data[i].day + ": " + data[i].recipeName);
				recipeCards[i].GetRecipeCardText().text = data[i].recipeName;
				recipeCards[i].GetRecipeData().recipeName = data[i].recipeName;
				recipeCards[i].GetRecipeData().day = data[i].day;
			}
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
			dialog.SetInfoMessage("Upload recipes?\n This will add all the ingredients from each recipe to the shopping list.");

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
				dialog.Hide();

				List<Recipe> recipesToUpload = CombineDuplicateRecipesFromRecipeCards();

				print("====\nThe recipes to upload are:\n====");

				foreach (Recipe r in recipesToUpload)
				{
					//print(r.ToString());

					List<string> items = new List<string>();

					foreach (RecipeIngredient ingredient in r.ingredients)
					{
						string item = ingredient.ingredient.name + " " + ingredient.amount + " " + ingredient.weight;
						print(item);
						items.Add(item);
					}
				}
			}
		}

		/// <summary>
		/// Iterates through the recipe cards, and combines duplicate recipes. <para/>
		/// E.g. If Pie & Peas occurs twice, then the returned list will contain double the amount of ingredients under one 
		/// recipe, instead of the same recipe occuring twice.
		/// </summary>
		private List<Recipe> CombineDuplicateRecipesFromRecipeCards()
		{
			List<Recipe> recipes = new List<Recipe>();
			foreach (RecipeCard card in recipeCards)
			{
				string recipeName = card.GetRecipeData().recipeName;

				Recipe recipeScriptableObject = RecipeDatabase.instance.FindRecipe(recipeName);

				// Could be a free text recipe
				if (recipeScriptableObject != null)
				{
					// Make a copy so we dont update the original scriptable object
					Recipe recipe = Instantiate(recipeScriptableObject);

					bool recipeAlreadyPresent = recipes.Where(x => x.name.Equals(recipe.name)).FirstOrDefault() != null ? true : false;
					if (recipeAlreadyPresent)
					{
						print(recipe.name + " is already in the recipes to upload");
						Recipe containedRecipe = recipes.Find(x => x.name.Equals(recipe.name));

						for (int i = 0; i < containedRecipe.ingredients.Length; i++)
						{
							RecipeIngredient containedRecipeIngredient = containedRecipe.ingredients[i];

							float currentAmount = containedRecipeIngredient.amount;
							float amountToAdd = recipe.ingredients[i].amount;
							float newAmount = currentAmount + amountToAdd;
							containedRecipeIngredient.amount = newAmount;

							print("The current added recipe contains " + currentAmount + " of " + containedRecipeIngredient.ingredient.name);
							print("The recipe to add contains " + amountToAdd + " of " + recipe.ingredients[i].ingredient.name);
							print("The new amount is now: " + containedRecipeIngredient.amount);
						}
					}
					else
					{
						print(recipe.name + " is NOT in the recipes to upload");
						recipes.Add(recipe);
					}
				}
			}
			return recipes;
		}
	}
}