using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class EnemyFactory
{
    private static string savePath = Path.Combine(Application.persistentDataPath, "Saves/knownBeasts.json");
    private static List<EnemySaveData> allBeasts = new List<EnemySaveData>();

    public static List<EnemySaveData> LoadBeasts()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            allBeasts = JsonUtility.FromJson<Wrapper>(json).list;
            return allBeasts;
        }
        return new List<EnemySaveData>();
    }

    public static void SaveBeasts(List<EnemySaveData> beasts)
    {
        Wrapper wrapper = new Wrapper { list = beasts };
        string json = JsonUtility.ToJson(wrapper, true);
        File.WriteAllText(savePath, json);
    }

    public static void IncBeastKillCount(string id, string zone)
    {
        bool found = false;
        foreach (EnemySaveData i in allBeasts)
        {
            if (i.id == id)
            {
                i.killCount += 1;
                found = true;
                break;
            }
        }
        if (!found)
        {
            EnemySaveData newBeast = new EnemySaveData { id = id, killCount = 1, habitats = new string[] { zone } };
            allBeasts.Add(newBeast);
        }

        SaveBeasts(allBeasts);
    }

    [System.Serializable]
    private class Wrapper { public List<EnemySaveData> list; }
}
