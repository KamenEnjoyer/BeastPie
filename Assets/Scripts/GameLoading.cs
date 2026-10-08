using System.IO;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using static ScenesConfig;

public class GameLoading : MonoBehaviour
{    
    public static GameLoading Instance;

    private void Awake()
    {
        Instance = this;
        string saveFolder = Path.Combine(Application.persistentDataPath, "Saves");
        string campSavePath = Path.Combine(saveFolder, "campStorage.json");
        string configSavePath = Path.Combine(saveFolder, "config.json");
         
        if (!Directory.Exists(saveFolder)) Directory.CreateDirectory(saveFolder);
        
        ConfigFactory.LoadConfig();
    }

    private void Start()
    {
        Locale locale = LocalizationSettings.AvailableLocales.GetLocale(currentLanguage.ToString());
        if (locale != null)
        {
            LocalizationSettings.SelectedLocale = locale;
        }
        SceneManager.LoadScene("HomeScene");
    }
}
