using UnityEngine;
using UnityEngine.UI;

public class RecipeCard : MonoBehaviour
{
	[SerializeField] private RecipeData recipeData;
	[SerializeField] private InputField recipeInput;

	public void SetRecipeName()
	{
		recipeData.recipeName = recipeInput.text;
	}

	public RecipeData GetRecipeData()
	{
		return recipeData;
	}

	public InputField GetRecipeInput()
	{
		return recipeInput;
	}
}

[System.Serializable]
public class RecipeData
{
	public string day;
	public string recipeName;
}

