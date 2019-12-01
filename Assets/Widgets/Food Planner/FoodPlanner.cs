using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;

using JsonHelper;

using Dialog;
using Dialog.OnlineLists;

using Recipes.UI;
using Recipes.ScriptableObjects;
using static Recipes.ScriptableObjects.Recipe.RecipeIngredient;
using static Recipes.ScriptableObjects.Recipe;
using Recipes;

namespace FoodPlannerWidget
{
	public class FoodPlanner : Widget
	{
		[SerializeField] private RecipeCard[] recipeCards;
		[SerializeField] private AddItemDialog shoppingList;
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

			WidgetLogger.instance.Log(this, "Saving recipes to file");

			StreamWriter writer = new StreamWriter(filePath, false);
			writer.WriteLine(json);
			writer.Close();

			WidgetLogger.instance.Log(this, "Done");
		}

		/// <summary>
		/// Loads the recipes from files
		/// </summary>
		private void LoadRecipesFromFile()
		{
			if (!File.Exists(filePath))
			{
				WidgetLogger.instance.Log(this, "Tried loading a recipe file, but it does not exist");
				SaveToFile();
			}

			string fileContent = File.ReadAllText(filePath);

			RecipeData[] data = FoodPlannerJsonHelper.FromJson(fileContent);

			for (int i = 0; i < data.Length; i++)
			{
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
			dialog.SetInfoMessage("Add all ingredients from each recipe to the shopping list?");

			while (dialog.GetResult() == DialogResult.NONE)
			{
				yield return null;
			}

			if (dialog.GetResult() == DialogResult.NO)
			{
				WidgetLogger.instance.Log(this, "Result of dialog was No");

				dialog.Hide();
				dialog.None();
				yield break;
			}

			if (dialog.GetResult() == DialogResult.YES)
			{
				WidgetLogger.instance.Log(this, "Result of dialog was Yes");

				dialog.Hide();

				List<Recipe> selectedRecipes = CollectSelectedRecipes();
				List<IngredientData> ingredientsToUpload = CollectedIngredientsFromRecipes(selectedRecipes);

				foreach (IngredientData s in ingredientsToUpload)
				{
					//print(s.ToString());
					shoppingList.AddItem(s.ToString());
				}
			}
		}

		/// <summary>
		/// Iterates through the recipe cards, finds the associated recipe by name and adds it to a list.
		/// </summary>
		private List<Recipe> CollectSelectedRecipes()
		{
			List<Recipe> selectedRecipes = new List<Recipe>();
			foreach (RecipeCard card in recipeCards)
			{
				string recipeName = card.GetRecipeData().recipeName;

				Recipe recipeScriptableObject = RecipeDatabase.instance.FindRecipe(recipeName);

				// Could be a free text recipe
				if (recipeScriptableObject != null)
				{
					// Make a copy so we dont update the original scriptable object
					Recipe recipe = Instantiate(recipeScriptableObject);
					selectedRecipes.Add(recipe);
				}
			}
			return selectedRecipes;
		}

		/// <summary>
		/// Collects ingredients from the selected recipes, removing duplicates and updating the amounts along the way.
		/// </summary>
		private List<IngredientData> CollectedIngredientsFromRecipes(List<Recipe> recipes)
		{
			List<IngredientData> ingredients = new List<IngredientData>();
			foreach (Recipe r in recipes)
			{
				foreach (RecipeIngredient ingredient in r.ingredients)
				{
					IngredientData ingredientData = ingredient.ToIngredientData();

					bool exists = ingredients.Any(x => x.name.Equals(ingredientData.name));

					if (exists)
					{
						IngredientData presentIngredient = ingredients.Where(x => x.name.Equals(ingredientData.name)).FirstOrDefault();
						IngredientData updatedIngredient = new IngredientData(ingredientData.name, (presentIngredient.amount + ingredientData.amount), ingredientData.weight);

						ingredients.Remove(presentIngredient);
						ingredients.Add(updatedIngredient);
					}
					else
					{
						ingredients.Add(ingredientData);
					}
				}
			}
			return ingredients;
		}
	}
}