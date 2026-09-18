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
        int lootDensityLimit = 0;
        int lootCount = 0;
        foreach (var ing in loot)
        {
            if (ing.GetIng() != null)
            {
                lootDensityLimit += ing.GetIng().data.densityLimit;
                lootCount++;
            }
        }
        ingredient.data.density = lootDensityLimit;
        ingredient.data.densityLimit = lootDensityLimit;
        if (lootCount < 3)
        {
            ShakeButton.Instance.Shake(loot[0].transform.parent, "Для жарки требуется 3 ингредиента.");
            return null;
        }
        ingredient.count = 1;
        return ingredient;
    }

    public int GetNeededCount()
    {
        return 2;
    }
}
