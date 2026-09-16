using System.Collections.Generic;
using UnityEngine;

public static class EffectRegistry
{
    //PotionEffectType
    public enum PET
    {
        GradualHeal
    }

    private static Dictionary<PET, PotionEffectInterface> effects =
        new Dictionary<PET, PotionEffectInterface>
        {
            { PET.GradualHeal, new GradualHeal() }
        };

    public static PotionEffectInterface Get(PET effect)
    {
        if (effects.TryGetValue(effect, out var effectScript)) return effectScript;

        Debug.LogError($"Potion effect '{effect}' not found");
        return null;
    }
}