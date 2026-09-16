using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DefaultIngredient")]
public class DefaultIngredientData : ScriptableObject
{
    public string id;
    public List<string> effectIds = new List<string>();
    public string[] dishEffectIds;
    public int defDensity;
    public int densityLimit;
    public int priceForOneDensity;
}
