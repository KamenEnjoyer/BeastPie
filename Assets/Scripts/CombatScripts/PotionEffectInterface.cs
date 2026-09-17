using NUnit.Framework;

public interface PotionEffectInterface
{
    void ApplyPotion(GILData potion);

    void ApplyDish(GILData dish);

    EffectData GetEffectData();
}