using UnityEngine;

public interface CatalystEffectInterface
{
    StorageContentData MixPotion(StorageContentData ingredient);

    StorageContentData MixDish(StorageContentData ingredient);

    int GetNeededCount();
}
