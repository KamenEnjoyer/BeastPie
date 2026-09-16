using NUnit.Framework;

public interface PotionEffectInterface
{
    void Apply(GILData potion);

    EffectData GetEffectData();
}