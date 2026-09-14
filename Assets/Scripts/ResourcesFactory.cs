using System.IO;
using UnityEngine;

public static class ResourcesFactory
{
    private static string savePath = Path.Combine(Application.persistentDataPath, "Saves/coins.json");
    private static int coinsQuantity;
    private static int traderCoinsQuantity;

    public static void LoadCoins()
    {
        if (!File.Exists(savePath))
        {
            coinsQuantity = 0;
            traderCoinsQuantity = 1000;
            SaveResources(coinsQuantity, traderCoinsQuantity);
        }
        else
        {
            string json = File.ReadAllText(savePath);
            coinsQuantity = JsonUtility.FromJson<Wrapper>(json).coins;
            traderCoinsQuantity = JsonUtility.FromJson<Wrapper>(json).traderCoins;
        }
        return;
    }

    public static int GetCoinsQuantity(bool isTrader = false)
    {
        return isTrader ? traderCoinsQuantity : coinsQuantity;
    }

    public static void AddCoins(int quantity, bool isTrader = false)
    {
        if (isTrader) traderCoinsQuantity += quantity; 
        else coinsQuantity += quantity;
        SaveResources(coinsQuantity, traderCoinsQuantity);
    }

    public static void SaveResources(int quantity, int traderQuantity)
    {
        Wrapper wrapper = new Wrapper { coins = quantity, traderCoins = traderQuantity };
        string json = JsonUtility.ToJson(wrapper, true);
        File.WriteAllText(savePath, json);
    }

    [System.Serializable]
    private class Wrapper { public int coins; public int traderCoins; }
}
