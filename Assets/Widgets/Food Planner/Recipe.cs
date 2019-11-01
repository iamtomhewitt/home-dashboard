using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Recipe", menuName = "Recipe")]
public class Recipe : ScriptableObject
{
    public RecipeIngredient[] ingredients;

    [System.Serializable]
    public class RecipeIngredient
    {
        public Ingredient ingredient;
        public float amount;
        public Weight weight;
    }

    public enum Weight
    {
        Grams,
        Teaspoon,
        Tablespoon,
        Quantity
    }
}