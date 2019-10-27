using UnityEngine;
using System.IO;

namespace FoodPlanner
{
	public class FoodPlanner : Widget
	{
		private RecipeCard[] recipes;
		private string filePath;

		private void Start()
		{
			filePath = Path.Combine(Application.persistentDataPath, "foodplanner.json");
			recipes = FindObjectsOfType<RecipeCard>();

			LoadRecipesFromFile();

			this.Initialise();
			InvokeRepeating("Run", 0f, RepeatRateInSeconds());
		}

		public override void Run()
		{

		}

		/// <summary>
		/// Saves the recipes to file.
		/// </summary>
		public void SaveToFile()
		{
			RecipeData[] data = new RecipeData[recipes.Length];

			for (int i = 0; i < recipes.Length; i++)
			{
				data[i] = recipes[i].GetRecipeData();
			}

			string json = FoodPlannerJsonHelper.ToJson(data);

			StreamWriter writer = new StreamWriter(filePath, false);
			writer.WriteLine(json);
			writer.Close();
			print(filePath);
		}

		/// <summary>
		/// Loads the recipes from files
		/// </summary>
		private void LoadRecipesFromFile()
		{
			string fileContent = File.ReadAllText(filePath);

			RecipeData[] data = FoodPlannerJsonHelper.FromJson(fileContent);

			for (int i = 0; i < data.Length; i++)
			{
				//print(data[i].day + ": " + data[i].recipeName);
				recipes[i].GetRecipeInput().text = data[i].recipeName;
				recipes[i].GetRecipeData().recipeName = data[i].recipeName;
				recipes[i].GetRecipeData().day = data[i].day;
			}
		}
	}
}