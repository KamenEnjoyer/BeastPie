using UnityEngine;
using System.IO;
using System.Collections.Generic;

public static class InventoryWithKeysFactory
{
    private static string savePath = Path.Combine(Application.persistentDataPath, "Saves/inventoryKeysSlots.json");
    private static List<InventorySaveData> bindings = new List<InventorySaveData>();

    public static List<InventorySaveData> LoadBindings()
    {
        if (!File.Exists(savePath))
        {
            List<InventorySaveData> defaults = CreateDefaultBindings();
            SaveAllBindings(defaults);
            bindings = defaults;
        }
        else
        {
            string json = File.ReadAllText(savePath);
            bindings = JsonUtility.FromJson<Wrapper>(json).list;
        }
        return bindings;
    }

    private static List<InventorySaveData> CreateDefaultBindings()
    {
        List<InventorySaveData> inventorySaveData = new List<InventorySaveData>();
        for (int i = 0; i < 8; i++)
        {
            string key = "Alpha" + (i + 1).ToString();
            if (i > 3)
            {
                switch (i)
                {
                    case 4: key = "Q"; break;
                    case 5: key = "E"; break;
                    case 6: key = "R"; break;
                    case 7: key = "F"; break;
                }
            }
            inventorySaveData.Add(new InventorySaveData
            {
                ingredientId = "",
                ingredientInStock = 0,
                key = key,
                slotIndex = i
            });
        }
        return inventorySaveData;
    }

    public static void SaveAllBindings(List<InventorySaveData> saveData)
    {
        Wrapper wrapper = new Wrapper { list = saveData };
        string json = JsonUtility.ToJson(wrapper, true);
        File.WriteAllText(savePath, json);
    }

    public static void SaveNamesByIngredient(InventorySaveData slotData)
    {
        bindings.Find(b => b.slotIndex == slotData.slotIndex).ingredientId = slotData.ingredientId;
        SaveAllBindings(bindings);
    }

    public static void SaveCountsByIngredient(string id, int count)
    {
        bindings.Find(b => b.ingredientId == id).ingredientInStock = count;
        SaveAllBindings(bindings);
    }

    [System.Serializable]
    private class Wrapper { public List<InventorySaveData> list; }
}
