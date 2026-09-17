using System.Collections.Generic;
using UnityEngine;

public static class EffectRegistry
{
    private static Dictionary<string, PotionEffectInterface> effects =
        new Dictionary<string, PotionEffectInterface>
        {
            { "GradualHeal", new GradualHeal() },
            { "Spicy", new Spicy() }
        };

    private static Dictionary<string, CatalystEffectInterface> catalystEffects =
        new Dictionary<string, CatalystEffectInterface>
        {
            { "water", new Water() },
            { "fire", new Fire() }
        };

    private static Dictionary<string, FoodEffectInterface> foodEffects =
        new Dictionary<string, FoodEffectInterface>
        {
            { "salt", new Salt() },
            { "ginger", new Ginger() }
        };

    public static PotionEffectInterface GetPotionEffect(string effect)
    {
        if (effects.TryGetValue(effect, out var effectScript)) return effectScript;

        Debug.LogError($"Potion effect '{effect}' not found");
        return null;
    }

    public static CatalystEffectInterface GetCatalystEffect(string effect)
    {
        if (catalystEffects.TryGetValue(effect, out var effectScript)) return effectScript;
        Debug.LogError($"Catalyst effect '{effect}' not found");
        return null;
    }

    public static FoodEffectInterface GetFoodEffect(string effect)
    {
        if (foodEffects.TryGetValue(effect, out var effectScript)) return effectScript;
        Debug.LogError($"Food effect '{effect}' not found");
        return null;
    }
}