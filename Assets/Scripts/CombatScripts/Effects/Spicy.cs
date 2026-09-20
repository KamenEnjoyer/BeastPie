using UnityEngine;
using System.Collections;
using System;

public class Spicy : PotionEffectInterface
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
        PlayerMovement.Instance.StartCoroutine(PlayerMovement.Instance.SpeedBoost(price/16f, density/3f));
    }

    public void ApplyDish()
    {
        PlayerMovement.Instance.moveSpeed = price / 100f;
    }

    public EffectData GetEffectData()
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
