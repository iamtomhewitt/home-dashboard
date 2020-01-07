using UnityEngine;
using UnityEngine.UI;

public class NewIngredientEntry : MonoBehaviour
{
	[SerializeField] private InputField ingredientName;
	[SerializeField] private InputField amount;
	[SerializeField] private Dropdown category;
	[SerializeField] private Dropdown weight;

	public string GetIngredientName()
	{
		return ingredientName.text;
	}

	public string GetAmount()
	{
		return amount.text;
	}

	public string GetCategory()
	{
		return category.options[category.value].text;
	}

	public string GetWeight()
	{
		return weight.options[weight.value].text;
	}
}
