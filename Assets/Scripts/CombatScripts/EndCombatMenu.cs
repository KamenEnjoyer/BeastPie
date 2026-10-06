using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndCombatMenu : MonoBehaviour
{
    public GameObject lootSlotPrefab;
    public Transform contentParent;

    private List<IngredientData> lootData;

    private bool canPressButton = false;

    private IEnumerator Start()
    {
        GetComponent<PlayerInput>().actions.FindActionMap("GamePlay").Disable();
        GetComponent<PlayerInput>().actions.FindActionMap("Menu").Enable();
        GetComponent<PlayerInput>().actions.FindActionMap("MenuManager").Enable();

        yield return null;
        Canvas.ForceUpdateCanvases();
        float width = contentParent.GetComponentInParent<ScrollRect>().GetComponent<RectTransform>().rect.width - 35f;
        contentParent.GetComponent<GridLayoutGroup>().cellSize = new Vector2(width / 5f, width * 0.25f);

        lootData = IngredientFactory.LoadIngredients("Loot");
        foreach (var slotData in lootData)
        {
            EndCombatMenuSlot slot = Instantiate(lootSlotPrefab, contentParent, false).GetComponent<EndCombatMenuSlot>();
            slot.Setup(slotData.id.Substring(0, slotData.id.IndexOf('_')), slotData.count);
        }

        int timeDelay = 60;
        while (timeDelay > 0)
        {
            timeDelay--;
            yield return null;
        }
        canPressButton = true;
    }

    public void OnExitToHome()
    {
        GoToNextScene("HomeScene", true);
    }

    public void OnExitToCamp()
    {
        GoToNextScene("HomeScene", false);
    }

    public void OnExitToNextCombat()
    {
        GoToNextScene("CombatScene", false);
    }

    private void GoToNextScene(string sceneName, bool isHome)
    {
        if (!canPressButton) return;
        ScenesConfig.IsHome = isHome;

        foreach (var loot in lootData)
        {
            IngredientFactory.AddIngredient(loot.id, loot.count, "Camp");
            GILFactory.AddIngredientFromDefaulds(loot.id, GILData.IngredientType.Loot);
        }
        PotionBarContent.Instance.SaveNewPotionCounts();
        IngredientFactory.DeleteFile(true);

        if (isHome)
        {
            foreach (var loot in IngredientFactory.LoadIngredients("Camp"))
            {
                IngredientFactory.AddIngredient(loot.id, loot.count, "Home");
            }
            IngredientFactory.DeleteFile(false);
        }

        Time.timeScale = 1f;
        GetComponent<PlayerInput>().actions.FindActionMap("GamePlay").Enable();
        GetComponent<PlayerInput>().actions.FindActionMap("Menu").Disable();
        GetComponent<PlayerInput>().actions.FindActionMap("MenuManager").Enable();

        SceneManager.LoadScene(sceneName);
    }
}
