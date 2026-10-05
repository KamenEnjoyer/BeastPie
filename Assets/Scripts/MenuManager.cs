using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;
    public GameObject menuPref;

    private GameObject currentMenu;

    void Awake()
    {
        Instance = this;
    }

    public void OnOpenHideMenu()
    {
        if (currentMenu) HideMenu();
        else ShowMenu();
    }

    public void ShowMenu()
    {
        GetComponent<PlayerInput>().SwitchCurrentActionMap("Menu");
        currentMenu = Instantiate(menuPref);
        Time.timeScale = 0f;
        Transform child = currentMenu.transform.Find("Background/Panel/ResumeButton");
        child.GetComponent<Button>().onClick.AddListener(HideMenu);
    }

    public void HideMenu()
    {
        Destroy(currentMenu);
        Time.timeScale = 1f;
    }
}
