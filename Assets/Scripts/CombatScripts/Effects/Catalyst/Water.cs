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
        int lootCount = 0;
        foreach (var ing in loot)
        {
            if (ing.GetIng() != null) lootCount++;
        }
        ingredient.data.density = Mathf.RoundToInt((ingredient.data.density) / lootCount);
        ingredient.count *= 3;
        return ingredient;
    }

    public int GetNeededCount()
    {
        return 1;
    }
}