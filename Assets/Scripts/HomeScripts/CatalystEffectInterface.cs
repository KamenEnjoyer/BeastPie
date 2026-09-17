using System.Collections.Generic;
using UnityEngine;

public interface CatalystEffectInterface
{
    StorageContentData MixPotion(StorageContentData ingredient);

    StorageContentData MixDish(StorageContentData ingredient, List<MixSlot> loot, List<MixSlot> food);

    int GetNeededCount();
}
