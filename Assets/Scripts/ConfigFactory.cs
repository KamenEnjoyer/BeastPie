using System.IO;
using UnityEngine;
using static ScenesConfig;

public class ConfigFactory : MonoBehaviour
{
    private static string savePath = Path.Combine(Application.persistentDataPath, "Saves/config.json");

    public static void LoadConfig()
    {
        if (!File.Exists(savePath))
        {
            IsHome = true;
            currentSorting = sortingVariables.Default;
            currentLanguage = languageVariables.en;
            SaveConfig();
        }
        else
        {
            string json = File.ReadAllText(savePath);
            IsHome = JsonUtility.FromJson<Wrapper>(json).isHome;
            currentSorting = JsonUtility.FromJson<Wrapper>(json).currentSorting;
            currentLanguage = JsonUtility.FromJson<Wrapper>(json).currentLanguage;
        }
        return;
    }

    public static void SaveConfig()
    {
        Wrapper wrapper = new Wrapper { isHome = IsHome, 
                                        currentSorting = currentSorting, 
                                        currentLanguage = currentLanguage };
        string json = JsonUtility.ToJson(wrapper, true);
        File.WriteAllText(savePath, json);
    }

    [System.Serializable]
    private class Wrapper { public bool isHome; 
                            public sortingVariables currentSorting; 
                            public languageVariables currentLanguage; }
}
