using System.Collections.Generic;
using UnityEngine;

public class Water : CatalystEffectInterface
{
    public StorageContentData MixPotion(StorageContentData ingredient)
    {
        if (ingredient.data.density > 0)
        {
            ingredient.data.density--;
            ingredient.count = 2;
            return ingredient;
        }
        return null;
    }

    public StorageContentData MixDish(StorageContentData ingredient, List<MixSlot> loot, List<MixSlot> food)
    {

        return ingredient;
    }

    public int GetNeededCount()
    {
        return 1;
    }
}