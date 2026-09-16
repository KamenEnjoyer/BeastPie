using UnityEngine;

public class WaterPotion : CatalystEffectInterface
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

    public StorageContentData MixDish(StorageContentData ingredient)
    {

        return ingredient;
    }

    public int GetNeededCount()
    {
        return 1;
    }
}