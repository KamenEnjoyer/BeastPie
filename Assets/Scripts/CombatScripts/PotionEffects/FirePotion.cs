using UnityEngine;

public class FirePotion : CatalystEffectInterface
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

    public StorageContentData MixDish(StorageContentData ingredient)
    {
        
        return ingredient;
    }

    public int GetNeededCount()
    {
        return 2;
    }
}
