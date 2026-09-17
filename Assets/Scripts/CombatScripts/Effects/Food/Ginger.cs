using UnityEngine;

public class Ginger : FoodEffectInterface
{
    public StorageContentData MixDish(StorageContentData ingredient)
    {
        ingredient.data.price += 30;
        ingredient.data.effectIds.Add("Spicy");
        return ingredient;
    }
}
