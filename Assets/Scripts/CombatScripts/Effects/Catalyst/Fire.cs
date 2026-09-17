using System.Collections.Generic;
using UnityEngine;

public class Fire : CatalystEffectInterface
{
    public StorageContentData MixPotion(StorageContentData ingredient)
    {
        if (ingredient.data.density < ingredient.data.densityLimit)
        {
            ingredient.data.density++;
            ingredient.count = 1;
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
        return 2;
    }
}
