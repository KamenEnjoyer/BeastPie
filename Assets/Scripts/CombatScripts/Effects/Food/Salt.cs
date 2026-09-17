using System.Collections.Generic;
using UnityEngine;

public class Salt : FoodEffectInterface
{
    public StorageContentData MixDish(StorageContentData ingredient)
    {
        ingredient.data.price += 20;
        return ingredient;
    }
}
