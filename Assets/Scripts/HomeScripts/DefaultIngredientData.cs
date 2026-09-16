using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DefaultIngredient")]
public class DefaultIngredientData : ScriptableObject
{
    public string id;
    public List<EffectRegistry.PET> effectIds = new List<EffectRegistry.PET>();
    public string[] dishEffectIds;
    public int defDensity;
    public int densityLimit;
    public int priceForOneDensity;
}
