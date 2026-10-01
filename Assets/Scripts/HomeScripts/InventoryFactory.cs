using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class InventoryFactory : MonoBehaviour
{
    private static string savePath = Path.Combine(Application.persistentDataPath, "Saves/inventorySlots.json");
    private static List<IngredientData> goods = new List<IngredientData>();

    public static List<IngredientData> LoadGoods()
    {
        if (!File.Exists(savePath))
        {
            SaveAllGoods(new List<IngredientData>());
        }
        else
        {
            string json = File.ReadAllText(savePath);
            goods = JsonUtility.FromJson<Wrapper>(json).list;
        }
        return goods;
    }

    public static void SaveAllGoods(List<IngredientData> saveData)
    {
        Wrapper wrapper = new Wrapper { list = saveData };
        string json = JsonUtility.ToJson(wrapper, true);
        File.WriteAllText(savePath, json);
    }

    public static void AddNewIngredient(IngredientData slotData)
    {
        foreach (var item in goods)
        {
            if (item.id == slotData.id)
            {
                item.count += slotData.count;
                SaveAllGoods(goods);
                return;
            }
        }
        goods.Add(slotData);
        SaveAllGoods(goods);
    }

    [System.Serializable]
    private class Wrapper { public List<IngredientData> list; }
}
