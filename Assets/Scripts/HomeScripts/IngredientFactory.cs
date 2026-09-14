using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class IngredientFactory
{
    private static string homeSavePath = Path.Combine(Application.persistentDataPath, "Saves/homeStorage.json");
    private static string campSavePath = Path.Combine(Application.persistentDataPath, "Saves/campStorage.json");
    private static string lootSavePath = Path.Combine(Application.persistentDataPath, "Saves/loot.json");

    private static string savePath;
    private static List<IngredientData> allIngredients = new List<IngredientData>();

    public static List<IngredientData> LoadIngredients(string loadLootCampOrHome)
    {
        if (loadLootCampOrHome == "Loot") savePath = lootSavePath;
        else if (loadLootCampOrHome == "Camp") savePath = campSavePath;
        else savePath = homeSavePath;
        //Debug.Log(savePath);
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            allIngredients = JsonUtility.FromJson<Wrapper>(json).list;
            return allIngredients;
        }
        return new List<IngredientData>();
    }

    public static void SaveIngredients(List<IngredientData> ingredients)
    {
        Wrapper wrapper = new Wrapper { list = ingredients };
        string json = JsonUtility.ToJson(wrapper, true);
        File.WriteAllText(savePath, json);
    }

    public static void AddIngredient(string id, int count, string addToLootCampOrHome)
    {
        if (addToLootCampOrHome == "Loot") savePath = lootSavePath;
        else if (addToLootCampOrHome == "Camp") savePath = campSavePath;
        else savePath = homeSavePath;

        allIngredients = LoadIngredients(addToLootCampOrHome); //Мне не нравится, что ингредиенты каждый раз нужно загружать. Можно попробовать загрузить один раз перед пачкой добавлений.

        bool found = false;
        foreach (IngredientData i in allIngredients)
        {
            if (i.id == id)
            {
                i.count += count;
                found = true;
                if (StorageContent.Instance != null) StorageContent.Instance.UpdateOneIngredient(id, i.count);
                break;
            }
        }
        if (!found)
        {
            IngredientData newIngredient = new IngredientData { id = id, count = count };
            if (StorageContent.Instance != null) StorageContent.Instance?.UpdateOneIngredient(id, count);
            allIngredients.Add(newIngredient);
        }

        SaveIngredients(allIngredients); //также и сохранять из раза в раз не особо обязательно
    }

    public static bool RemoveIngredient(string id, int count, bool isHome)
    {
        if (isHome) savePath = homeSavePath;
        else savePath = campSavePath;
        allIngredients = LoadIngredients(isHome == true ? "Home" : "Camp"); //Очередная подгрузка элементов
        foreach (IngredientData i in allIngredients)
        {
            if (i.id == id)
            {
                i.count = Mathf.Max(0, i.count - count);
                if (StorageContent.Instance != null)
                {
                    StorageContent.Instance.UpdateOneIngredient(id, i.count);
                }
                SaveIngredients(allIngredients);
                return true;
            }
        }
        Debug.LogWarning("Ingredient with ID " + id + " not found.");
        return false;
    }

    public static int GetCountById(string id) //ПО ИДЕЕ ДОЛЖНО РАБОТАТЬ САМО ПО СЕБЕ, В ЗАВИСИМОСТИ ОТ ТОГО, КАКАЯ СЦЕНА — ДОМ ИЛИ ЛАГЕРЬ, ОН ПОДГРУЖАЕТ НУЖНОЕ
    {
        foreach (var ingredient in allIngredients)
        {
            if (ingredient.id == id) return ingredient.count;
        }
        Debug.LogError("Ingredient with ID " + id + " not found.");
        return 0;
    }

    public static void DeleteFile(bool deleteCombat)
    {
        if (deleteCombat) savePath = lootSavePath;
        else savePath = campSavePath;
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
        }
    }

    [System.Serializable]
    private class Wrapper { public List<IngredientData> list; }
}
