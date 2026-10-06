using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;
    public GameObject menuPref;
    public PlayerInput otherPlayerInput;

    private GameObject currentMenu;

    void Awake()
    {
        Instance = this;
    }

    public void OnOpenMenu()
    {
        GetComponent<PlayerInput>().SwitchCurrentActionMap("Menu");
        otherPlayerInput.SwitchCurrentActionMap("Menu");

        currentMenu = Instantiate(menuPref);
        Time.timeScale = 0f;
        Transform child = currentMenu.transform.Find("Background/Panel/ResumeButton");
        child.GetComponent<Button>().onClick.AddListener(OnHideMenu);
    }

    public void OnHideMenu()
    {
        GetComponent<PlayerInput>().SwitchCurrentActionMap("GamePlay");
        otherPlayerInput.SwitchCurrentActionMap("GamePlay");

        Destroy(currentMenu);
        Time.timeScale = 1f;
    }
}
