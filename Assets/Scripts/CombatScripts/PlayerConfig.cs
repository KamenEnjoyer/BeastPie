using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class PlayerConfig
{
    private static string savePath = Path.Combine(Application.persistentDataPath, "Saves/playerConfig.json");
    
    public static List<EffectData> LoadDish()
    {
        if (!File.Exists(savePath))
        {
            SaveResources(new List<EffectData>());
            return null;
        }
        else
        {
            string json = File.ReadAllText(savePath);
            return JsonUtility.FromJson<Wrapper>(json).effects;
        }
    }

    public static void SetNewDish(string id)
    {
        GILData dish = GILFactory.FindIngredientById(id);
        if (dish == null) Debug.LogError("CAN'T FIND DISH IN PlayerConfig.");
        else if (dish.effectIds == null || dish.effectIds.Count == 0) Debug.LogError("CAN'T FIND DISH EFFECTS IN PlayerConfig.");
        else
        {
            List<EffectData> effects = new List<EffectData>();
            foreach (var effId in dish.effectIds)
            {
                PotionEffectInterface effect = EffectRegistry.GetPotionEffect(effId);
                if (effect == null) Debug.LogError("Effect " + effId + " in PotionBarSlot not found.");
                else
                {
                    effect.Setup(dish.density, dish.price);
                    effects.Add(effect.GetEffectData());
                }
            }
            SaveResources(effects);
        }
    }

    public static void SaveResources(List<EffectData> effects)
    {
        Wrapper wrapper = new Wrapper { effects = effects };
        string json = JsonUtility.ToJson(wrapper, true);
        File.WriteAllText(savePath, json);
    }

    [System.Serializable]
    private class Wrapper { public List<EffectData> effects; }
}
