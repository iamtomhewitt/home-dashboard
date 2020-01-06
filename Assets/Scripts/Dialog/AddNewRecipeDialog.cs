using UnityEngine;

namespace Dialog
{
	public class AddNewRecipeDialog : MonoBehaviour
	{
		[SerializeField] private GameObject newIngredientEntry;
		[SerializeField] private Transform newIngredientsContent;

		/// <summary>
		/// Called from a Unity button to add a new ingredient to the add new recipe dialog.
		/// </summary>
		public void AddNewIngredientEntry()
		{
			Instantiate(newIngredientEntry, newIngredientsContent);
		}
	}
}
