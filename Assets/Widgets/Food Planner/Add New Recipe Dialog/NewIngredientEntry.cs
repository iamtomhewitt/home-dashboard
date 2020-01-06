using UnityEngine;
using UnityEngine.UI;

public class NewIngredientEntry : MonoBehaviour
{
	[SerializeField] private InputField ingredientName;
	[SerializeField] private InputField quantity;
	[SerializeField] private Dropdown category;
	[SerializeField] private Dropdown weight;

	public string GetIngredientName()
	{
		return ingredientName.text;
	}

	public string GetQuantity()
	{
		return quantity.text;
	}

	public string GetCategory()
	{
		return category.itemText.text;
	}

	public string GetWeight()
	{
		return weight.itemText.text;
	}
}
