using UnityEngine;

[CreateAssetMenu(menuName = "DefaultIngredient")]
public class DefaultIngredientData : ScriptableObject
{
    public string id;
    public string[] effectIds;
    public int defDensity;
    public int densityLimit;
    public int priceForOneDensity;
}
