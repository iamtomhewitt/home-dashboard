using UnityEngine;

namespace Recipes.ScriptableObjects
{
	[System.Serializable]
	[CreateAssetMenu(fileName = "New Recipe", menuName = "Recipe")]
	public class Recipe : ScriptableObject
	{
		public int serves;
		public RecipeIngredient[] ingredients;

		public override string ToString()
		{
			string s = "Name: " + name + "\nServes: " + serves + "\nIngredients:";

			foreach (RecipeIngredient i in ingredients)
			{
				s += "\t" + i.ToString();
			}

			return s;
		}

		[System.Serializable]
		public class RecipeIngredient
		{
			public Ingredient ingredient;
			public float amount;
			public Weight weight;

			public override string ToString()
			{
				return "\nName: " + ingredient.GetName() + "\n\tType: " + ingredient.GetType() + "\n\tAmount: " + amount + "\n\tWeight: " + weight;
			}

			public IngredientData ToIngredientData()
			{
				return new IngredientData(this.ingredient.name, this.amount, this.weight.ToString());
			}

			public struct IngredientData
			{
				public string name;
				public float amount;
				public string weight;

				public IngredientData(string name, float amount, string weight)
				{
					this.name = name;
					this.amount = amount;
					this.weight = weight;
				}

				public override string ToString()
				{
					return this.name + " (" + this.amount + (this.weight.Equals("quantity") ? "" : " " + this.weight) + ")";
				}
			}
		}

		public enum Weight
		{
			grams,
			tsp,
			tbsp,
			quantity,
			ml
		}
	}
}