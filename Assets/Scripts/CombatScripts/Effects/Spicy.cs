using UnityEngine;
using System.Collections;
using System;

public class Spicy : PotionEffectInterface
{
    private int density;
    private int price;

    public void ApplyPotion(GILData potion)
    {
        density = potion.density;
        price = potion.price;
        PlayerMovement.Instance.StartCoroutine(PlayerMovement.Instance.SpeedBoost(price/16f, density/3f));
    }

    public void ApplyDish(GILData dish)
    {
        density = dish.density;
        price = dish.price;
        PlayerMovement.Instance.moveSpeed = price / 100f;
    }

    public EffectData GetEffectData()
    {
        EffectData effect = ScriptableObject.CreateInstance<EffectData>();

        effect.efcName = "Spicy";
        effect.cooldownDuration = density;
        effect.screenCount = Mathf.RoundToInt(density / 5f);
        effect.price = price;

        return effect;
    }
}
