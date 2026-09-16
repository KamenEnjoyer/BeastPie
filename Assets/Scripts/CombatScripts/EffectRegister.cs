using System.Collections.Generic;
using UnityEngine;

public static class EffectRegistry
{
    private static Dictionary<string, PotionEffectInterface> effects =
        new Dictionary<string, PotionEffectInterface>
        {
            { "GradualHeal", new GradualHeal() }
        };

    public static PotionEffectInterface Get(string effect)
    {
        if (effects.TryGetValue(effect, out var effectScript)) return effectScript;

        Debug.LogError($"Potion effect '{effect}' not found");
        return null;
    }
}