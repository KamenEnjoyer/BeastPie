using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;
    public GameObject menuPref;

    private GameObject currentMenu;

    void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentMenu) HideMenu();
            else ShowMenu();
        }
    }

    public void ShowMenu()
    {
        currentMenu = Instantiate(menuPref);
        Transform child = currentMenu.transform.Find("Background/Panel/ResumeButton");
        child.GetComponent<Button>().onClick.AddListener(HideMenu);
        child = currentMenu.transform.Find("Background/Panel/ExitButton");
        child.GetComponent<Button>().onClick.AddListener(ExitGame);
    }

    public void HideMenu()
    {
        Destroy(currentMenu);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
