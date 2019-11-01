using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Ingredient", menuName = "Ingredient")]
public class Ingredient : ScriptableObject
{
    [SerializeField] private string name;
    [SerializeField] private Type type;

    public enum Type
    {
        Meat,
        Vegetables,
        Dairy,
        Frozen,
        Other
    }
}