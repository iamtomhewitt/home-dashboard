using UnityEngine;

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
	}

	public enum Weight
	{
		Grams,
		Teaspoon,
		Tablespoon,
		Quantity,
		Millilitres
	}
}