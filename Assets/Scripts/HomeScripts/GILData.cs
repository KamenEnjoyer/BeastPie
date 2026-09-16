//Global Ingredients List
using System.Collections.Generic;
[System.Serializable]
public class GILData
{
    public enum IngredientType { Catalyst, Loot, Potion, Food, Dish, Other };
    public IngredientType type = IngredientType.Catalyst;

    public string id;

    public string ingName;
    public string description;
    public string effect;

    public List<string> effectIds = new List<string>();
    public List<string> dishEffectIds = new List<string>();
    public int density;
    public int densityLimit;
    public int price;
    public List<IngredientData> recipe = new List<IngredientData>();
    public List<string> conflictIds = new List<string>();
}