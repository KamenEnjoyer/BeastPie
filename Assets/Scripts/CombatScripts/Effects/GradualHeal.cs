using UnityEngine;

public class GradualHeal : PotionEffectInterface
{
    private int density;
    private int price;

    public void Setup(int density, int price)
    {
        this.density = density;
        this.price = price;
    }

    public void ApplyPotion()
    {
        PlayerHealth.Instance.StartCoroutine(PlayerHealth.Instance.GradualHeal(density*10, density));
    }

    public void ApplyDish()
    {
        PlayerHealth.Instance.maxHealth += price/2;
    }

    public EffectData GetEffectData()
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
