using UnityEngine;
using System.IO;
using JsonHelper;
using Dialog;
using System.Collections;
using System.Collections.Generic;
using static Recipe;
using System.Linq;
using OnlineLists;
using static Recipe.RecipeIngredient;

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

				List<Recipe> selectedRecipes = CollectSelectedRecipes();
				List<IngredientData> ingredientsToUpload = CollectedIngredientsFromRecipes(selectedRecipes);

				print("===");
				foreach (IngredientData s in ingredientsToUpload)
				{
					print(s.ToString());
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
						//print("The existing ingredient is: " + presentIngredient.ToString());
						//print("The ingredient trying to add is: " + ingredientData.ToString());

						IngredientData updatedIngredient = new IngredientData(ingredientData.name, (presentIngredient.amount + ingredientData.amount));
						//print("The updated ingredient will be: " + updatedIngredient.ToString());

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