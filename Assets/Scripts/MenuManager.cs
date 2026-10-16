using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;
    public GameObject menuPref;
    public PlayerInput otherPlayerInput;

    private PlayerInput playerInput;

    private GameObject currentMenu;

    void Awake()
    {
        Instance = this;
        playerInput = GetComponent<PlayerInput>();
    }

    public void OnOpenMenu()
    {
        ToggleActionMap(true);

        currentMenu = Instantiate(menuPref);
        Time.timeScale = 0f;
        Transform child = currentMenu.transform.Find("Background/Panel/ResumeButton");
        child.GetComponent<Button>().onClick.AddListener(OnHideMenu);
    }

    public void OnHideMenu()
    {
        ToggleActionMap(false);

        Destroy(currentMenu);
        Time.timeScale = 1f;
    }

    public void ToggleActionMap(bool openMenu)
    {
        if (openMenu)
        {
            playerInput.SwitchCurrentActionMap("Menu");
            otherPlayerInput.SwitchCurrentActionMap("Menu");
        }
        else
        {
            playerInput.SwitchCurrentActionMap("GamePlay");
            otherPlayerInput.SwitchCurrentActionMap("GamePlay");
        }
    }
}
