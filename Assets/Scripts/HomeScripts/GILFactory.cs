//Global Ingredients List
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization.Settings;

public static class GILFactory
{
    private static string savePath = Path.Combine(Application.persistentDataPath, "Saves/globalIngredientsList.json");
    private static List<GILData> allIngredients = new List<GILData>();

    public static List<GILData> LoadIngredients()
    {
        Debug.Log("Loading ingredients from: " + savePath);
        if (!File.Exists(savePath))
        {
            List<GILData> defaults = CreateDefaultIngredients();
            allIngredients = defaults;
        }
        else
        {
            string json = File.ReadAllText(savePath);
            allIngredients = JsonUtility.FromJson<Wrapper>(json).list;
        }
        return allIngredients;
    }

    public static void UpdateLocalization()
    {
        foreach (GILData data in allIngredients)
        {
            if (data.type == GILData.IngredientType.Potion) continue;
            if (data.type != GILData.IngredientType.Loot)
            {
                SetLocalization(data.id, out data.ingName, out data.description, out data.effect);
                continue;
            }
            string substringId = data.id.Substring(0, data.id.Length - 2);
            SetLocalization(substringId, out data.ingName, out data.description, out data.effect);
        }
        SaveIngredients(allIngredients);
    }

    public static void SetLocalization(string locId, out string name, out string description, out string effect)
    {
        name = LocalizationSettings.StringDatabase.GetLocalizedString("IngredientsNamesLocalization", locId);
        description = LocalizationSettings.StringDatabase.GetLocalizedString("IngredientsDescriptionsLocalization", locId);
        effect = LocalizationSettings.StringDatabase.GetLocalizedString("IngredientsEffectsLocalization", locId);
    }

    public static void SaveIngredients(List<GILData> ingredients)
    {
        Wrapper wrapper = new Wrapper { list = ingredients };
        string json = JsonUtility.ToJson(wrapper, true);
        File.WriteAllText(savePath, json);
    }

    public static void CreateIngredient(GILData newIngredient)
    {
        bool ingredientExist = false;
        foreach (GILData i in allIngredients)
        {
            if (i.id == newIngredient.id)
            {
                ingredientExist = true;
                break;
            }
        }
        if (!ingredientExist)
        {
            allIngredients.Add(newIngredient);
        }

        SaveIngredients(allIngredients);
    }

    private static List<GILData> CreateDefaultIngredients()
    {
        List<GILData> defaults = new List<GILData>
        {
            new GILData
            {
                type = GILData.IngredientType.Catalyst,
                id = "water",
                density = 0,
                densityLimit = 0,
                effectIds = new List<string> { "water" }
            },
            new GILData
            {
                type = GILData.IngredientType.Catalyst,
                id = "fire",
                density = 10,
                densityLimit = 10,
                effectIds = new List<string> { "fire" }
            }
        };
        foreach (GILData data in defaults)
        {
            SetLocalization(data.id, out data.ingName, out data.description, out data.effect);
        }
        SaveIngredients(defaults);
        return defaults;
    }

    public static void AddIngredientFromDefaulds(string id, GILData.IngredientType type)
    {
        if(FindIngredientById(id) != null) return;

        string substringId;
        if (type == GILData.IngredientType.Loot) substringId = id.Substring(0, id.Length - 2);
        else substringId = id;

        GILData newIngredient = new GILData
        {
            id = id,
            type = type
        };
        SetLocalization(substringId, out newIngredient.ingName, out newIngredient.description, out newIngredient.effect);

        if(type == GILData.IngredientType.Loot)
        {
            DefaultIngredientData defaultLootData = Resources.Load<DefaultIngredientData>("IngredientsTypes/Loot/" + substringId);
            if (defaultLootData != null)
            {
                newIngredient.effectIds = new List<string>(defaultLootData.effectIds);
                newIngredient.density = defaultLootData.defDensity;
                newIngredient.densityLimit = defaultLootData.densityLimit;
                newIngredient.price = defaultLootData.priceForOneDensity;
                allIngredients.Add(newIngredient);
                SaveIngredients(allIngredients);
            }
            return;
        }

        if (type == GILData.IngredientType.Food)
        {
            FoodIngredientData defaultFoodData = Resources.Load<FoodIngredientData>("IngredientsTypes/Food/" + substringId);
            if (defaultFoodData != null)
            {
                newIngredient.effectIds = new List<string>(defaultFoodData.effectIds);
                newIngredient.conflictIds = new List<string>(defaultFoodData.conflictIds);
                newIngredient.price = defaultFoodData.price;
                allIngredients.Add(newIngredient);
                SaveIngredients(allIngredients);
            }
            return;
        }

        Debug.LogError("Default ingredient with ID " + id + " not found in Resources/Ingredients/" + type.ToString() + ".");
    }

    public static GILData FindIngredientById(string id) //Возможно стоит добавить сотировку по типу
    {
        foreach (var ingredient in allIngredients)
        {
            if (ingredient.id == id) return ingredient;
        }
        Debug.Log("Ingredient with ID " + id + " not found.");
        return null;
    }

    public static string FindIdForNewIngredient(GILData ingredient, GILData.IngredientType type)
    {
        int maxId = 0;
        foreach (var item in allIngredients)
        {
            if(item.type == type)
            {
                if (item.recipe.Count == ingredient.recipe.Count)
                {
                    var dict = new Dictionary<string, int>();
                    foreach (var ing in item.recipe)
                    {
                        if (!dict.ContainsKey(ing.id)) dict[ing.id] = 0;
                        dict[ing.id] += ing.count;
                    }
                    bool match = true;
                    foreach (var ing in ingredient.recipe)
                    {
                        if (!dict.ContainsKey(ing.id))
                        {
                            match = false;
                            break;
                        }
                        dict[ing.id] -= ing.count;

                        if (dict[ing.id] < 0)
                        {
                            match = false;
                            break;
                        }
                    }

                    foreach (var kvp in dict)
                    {
                        if (kvp.Value != 0)
                        {
                            match = false;
                            break;
                        }
                    }

                    if (match) return item.id.Replace(type.ToString(), "");
                }
                if (int.TryParse(item.id.Replace(type.ToString(), ""), out int value))
                {
                    if (value > maxId) maxId = value;
                }
            }
        }

        Debug.Log("ID for the given ingredient not found.");
        return (maxId + 1).ToString();
    }

    public static void UpdatePotionData(GILData potion)
    {
        for (int i = 0; i < allIngredients.Count; i++)
        {
            if (allIngredients[i].id == potion.id)
            {
                allIngredients[i] = potion;
                SaveIngredients(allIngredients);
                return;
            }
        }
        Debug.LogError("Potion with ID " + potion.id + " not found in the global ingredients list.");
    }

    [System.Serializable]
    private class Wrapper { public List<GILData> list; }
}
