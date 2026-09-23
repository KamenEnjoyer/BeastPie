using UnityEngine;

public class GradualHeal : PotionEffect
{
    public override void ApplyPotion()
    {
        PlayerHealth.Instance.StartCoroutine(PlayerHealth.Instance.GradualHeal(density*10, density));
    }

    public override void ApplyDish()
    {
        PlayerHealth.Instance.maxHealth += price / 2f;
    }

    public override EffectData GetEffectData()
    {
        EffectData effect = new EffectData();

        effect.efcName = "GradualHeal";
        effect.cooldownDuration = density;
        effect.screenCount = Mathf.RoundToInt(density / 5f);
        effect.maxScreenCount = effect.screenCount;
        effect.price = price;

        return effect;
    }
}
