using UnityEngine;

public class MenuSpawner : MonoBehaviour
{
    public static MenuSpawner Instance;

    public Transform screenParent;
    public GameObject deathPanelPref;
    public GameObject victoryPanelPref;

    private void Awake()
    {
        Instance = this;
    }

    public void ShowDeathMenu()
    {
        Instantiate(deathPanelPref, screenParent);
        Time.timeScale = 0f;
    }

    public void ShowVictoryMenu()
    {
        Instantiate(victoryPanelPref, screenParent);
        Time.timeScale = 0f;
    }
}
