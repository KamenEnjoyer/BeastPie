using UnityEngine;
using UnityEngine.SceneManagement;

public class InventoryToCombatButton : MonoBehaviour
{
    public void OnButtonClicked()
    {
        foreach (var ingredient in InventoryFactory.LoadGoods(true))
        {
            if (ingredient.slotData.id == "") continue;
            ingredient.slotData.count = IngredientFactory.GetCountById(ingredient.slotData.id); //ОТКУДА ДАННЫЕ, ИЗ ДОМА ИЛИ ЛАГЕРЯ?
            InventoryFactory.SaveCountsByIngredient(ingredient.slotData.id, ingredient.slotData.count, true);
        }
        SceneManager.LoadScene("MapScene");
    }
}
