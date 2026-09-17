using UnityEngine;

public class GradualHeal : PotionEffectInterface
{
    private int density;
    private int price;
    public void ApplyPotion(GILData potion)
    {
        density = potion.density;
        price = potion.price;
        PlayerHealth.Instance.StartCoroutine(PlayerHealth.Instance.GradualHeal(density*10, density));
    }

    public void ApplyDish(GILData dish)
    {

    }

    public EffectData GetEffectData()
    {
        EffectData effect = ScriptableObject.CreateInstance<EffectData>();

        effect.efcName = "GradualHeal";
        effect.cooldownDuration = density;
        effect.screenCount = Mathf.RoundToInt(density / 5f);
        effect.price = price;

        return effect;
    }
}
