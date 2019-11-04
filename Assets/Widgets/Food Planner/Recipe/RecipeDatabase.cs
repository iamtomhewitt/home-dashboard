using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RecipeDatabase : MonoBehaviour
{
	[SerializeField] private Recipe[] recipes;

	public static RecipeDatabase instance;

	private void Start()
	{
		instance = this;
	}

	public Recipe[] GetRecipes()
	{
		return recipes;
	}

	public List<string> GetRecipeNames()
	{
		return recipes.Select(x => x.name).ToList();
	}
}