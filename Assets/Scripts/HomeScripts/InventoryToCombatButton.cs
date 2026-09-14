using UnityEngine;
using UnityEngine.SceneManagement;

public class InventoryToCombatButton : MonoBehaviour
{
    public void OnButtonClicked()
    {
        foreach (var ingredient in InventoryFactory.LoadBindings())
        {
            if (ingredient.ingredientId == "") continue;
            ingredient.ingredientInStock = IngredientFactory.GetCountById(ingredient.ingredientId); //ОТКУДА ДАННЫЕ, ИЗ ДОМА ИЛИ ЛАГЕРЯ?
            InventoryFactory.SaveCountsByIngredient(ingredient.ingredientId, ingredient.ingredientInStock);
        }
        SceneManager.LoadScene("CombatScene");
    }
}
