using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoading : MonoBehaviour
{    
    public static GameLoading Instance;
    private string campSavePath;
    private string saveFolder;

    private void Awake()
    {
        Instance = this;
        saveFolder = Path.Combine(Application.persistentDataPath, "Saves");
        campSavePath = Path.Combine(saveFolder, "campStorage.json");

        if (!Directory.Exists(saveFolder)) Directory.CreateDirectory(saveFolder);
        if (!File.Exists(campSavePath)) ScenesConfig.IsHome = true;
        else ScenesConfig.IsHome = false;
    }

    private void Start()
    {
        SceneManager.LoadScene("HomeScene");
    }
}
