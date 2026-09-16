using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CatalystIngredient")]
public class CatalystIngredientData : ScriptableObject
{
    public string id;
    public string[] effectIds;
    public string[] dishEffectIds;
    public int price;
    public int minQuantity;
    public int maxQuantity;
}
