using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndCombatMenu : MonoBehaviour
{
    public GameObject lootSlotPrefab;
    public Transform contentParent;

    private List<IngredientData> lootData;

    public void Start()
    {
        lootData = IngredientFactory.LoadIngredients("Loot");
        foreach (var slotData in lootData)
        {
            EndCombatMenuSlot slot = Instantiate(lootSlotPrefab, contentParent, false).GetComponent<EndCombatMenuSlot>();
            slot.Setup(slotData.id, slotData.count);
        }
    }

    public void ExitToHome()
    {
        GoToNextScene("HomeScene", true);
    }

    public void ExitToCamp()
    {
        GoToNextScene("HomeScene", false);
    }

    public void ExitToNextCombat()
    {
        GoToNextScene("CombatScene", false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitToHome();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ExitToNextCombat();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            ExitToCamp();
        }
    }

    private void GoToNextScene(string sceneName, bool isHome)
    {
        Time.timeScale = 1f;
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
        
        SceneManager.LoadScene(sceneName);
    }
}
