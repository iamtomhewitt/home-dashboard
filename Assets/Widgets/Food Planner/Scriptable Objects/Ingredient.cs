using UnityEngine;

namespace Recipes.ScriptableObjects
{
	[System.Serializable]
	[CreateAssetMenu(fileName = "New Ingredient", menuName = "Ingredient")]
	public class Ingredient : ScriptableObject
	{
		[SerializeField] private string name;
		[SerializeField] private Type type;

		public string GetName()
		{
			return name;
		}

		public Type GetType()
		{
			return type;
		}

		public enum Type
		{
			Meat,
			Vegetables,
			Dairy,
			Frozen,
			Other
		}
	}
}