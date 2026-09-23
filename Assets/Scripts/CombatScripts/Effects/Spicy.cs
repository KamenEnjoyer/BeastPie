using UnityEngine;
using System.Collections;
using System;

public class Spicy : PotionEffect
{
    public override void ApplyPotion()
    {
        PlayerMovement.Instance.StartCoroutine(PlayerMovement.Instance.SpeedBoost(price/16f, density/3f));
    }

    public override void ApplyDish()
    {
        PlayerMovement.Instance.moveSpeed += price / 100f;
    }

    public override EffectData GetEffectData()
    {
        EffectData effect = new EffectData();

        effect.efcName = "Spicy";
        effect.cooldownDuration = density;
        effect.screenCount = Mathf.RoundToInt(density / 5f);
        effect.maxScreenCount = effect.screenCount;
        effect.price = price;

        return effect;
    }
}
