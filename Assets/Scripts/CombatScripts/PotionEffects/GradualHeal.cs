using UnityEngine;

public class GradualHeal : PotionEffectInterface
{
    private int density;
    public void Apply(GILData potion)
    {
        density = potion.density;
        PlayerHealth.Instance.StartCoroutine(PlayerHealth.Instance.GradualHeal(density*10, density));
    }

    public EffectData GetEffectData()
    {
        EffectData effect = ScriptableObject.CreateInstance<EffectData>();

        effect.efcName = "GradualHeal";
        effect.cooldownDuration = density;

        return effect;
    }
}
