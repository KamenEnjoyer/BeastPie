using UnityEngine;

[CreateAssetMenu(menuName = "DefaultFood")]
public class FoodIngredientData : ScriptableObject
{
    public string id;
    public string[] dishEffectIds;
    public string[] conflictIds;
    public int price;
    public int minQuantity;
    public int maxQuantity;
}
