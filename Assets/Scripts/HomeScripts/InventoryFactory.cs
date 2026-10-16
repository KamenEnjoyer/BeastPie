using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class InventoryFactory
{
    private static string playerSavePath = Path.Combine(Application.persistentDataPath, "Saves/inventoryPlayerSlots.json");
    private static string tigerSavePath = Path.Combine(Application.persistentDataPath, "Saves/inventoryTigerSlots.json");

    [Serializable]
    public struct InventoryData
    {
        public IngredientData slotData;
        public int slotIndex;
    }

    public static List<InventoryData> LoadGoods(bool playerInventory)
    {
        string savePath = playerInventory ? playerSavePath : tigerSavePath;
        List<InventoryData> goods = new List<InventoryData>();
        if (!File.Exists(savePath))
        {
            goods = CreateDefaultSlots();
            SaveAllGoods(savePath, goods);
        }
        else
        {
            string json = File.ReadAllText(savePath);
            goods = JsonUtility.FromJson<Wrapper>(json).list;
        }
        return goods;
    }

    private static List<InventoryData> CreateDefaultSlots()
    {
        List<InventoryData> inventoryData = new List<InventoryData>();
        for (int i = 0; i < 8; i++)
        {
            inventoryData.Add(new InventoryData
            {
                slotData = new IngredientData { id = "", count = 0 },
                slotIndex = i
            });
        }
        return inventoryData;
    }

    public static void SaveAllGoods(string savePath, List<InventoryData> goods)
    {
        Wrapper wrapper = new Wrapper { list = goods };
        string json = JsonUtility.ToJson(wrapper, true);
        File.WriteAllText(savePath, json);
    }

    public static void SaveDataByIndex(IngredientData data, int index, bool playerInventory)
    {
        List<InventoryData> goods = new List<InventoryData>();
        goods = LoadGoods(playerInventory);
        goods.Find(b => b.slotIndex == index).slotData.id = data.id;
        goods.Find(b => b.slotIndex == index).slotData.count = data.count;

        string savePath = playerInventory ? playerSavePath : tigerSavePath;
        SaveAllGoods(savePath, goods);
    }

    public static void SaveCountsByIngredient(string id, int count, bool playerInventory)
    {
        List<InventoryData> goods = new List<InventoryData>();
        goods = LoadGoods(playerInventory);
        goods.Find(b => b.slotData.id == id).slotData.count = count;

        string savePath = playerInventory ? playerSavePath : tigerSavePath;
        SaveAllGoods(savePath, goods);
    }

    [System.Serializable]
    private class Wrapper { public List<InventoryData> list; }
}
