using UnityEngine;
using System.Collections;

public class FoodPlannerJsonHelper : MonoBehaviour
{
	public static string ToJson(RecipeData[] recipes)
	{
		Wrapper<RecipeData> wrapper = new Wrapper<RecipeData>();
		wrapper.recipes = recipes;
		return JsonUtility.ToJson(wrapper);
	}

	public static RecipeData[] FromJson(string json)
	{
		Wrapper<RecipeData> wrapper = JsonUtility.FromJson<Wrapper<RecipeData>>(json);
		return wrapper.recipes;
	}

	[System.Serializable]
	private class Wrapper<T>
	{
		public T[] recipes;
	}
}
