using NUnit.Framework;

public interface PotionEffectInterface
{
    void Setup(int density, int price);

    void ApplyPotion();

    void ApplyDish();

    EffectData GetEffectData();
}