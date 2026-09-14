using UnityEngine;

public class MixDropZone : MonoBehaviour
{
    public enum DropZone { LeftPotion, RightPotion, MiddlePotion, LeftDish, RightDish, MiddleDish }
    public DropZone zone;

    public void OnIngredientDropped(StorageContentData ingredient)
    {
        switch (zone)
        {
            case DropZone.LeftPotion:
                MixPotionMode.Instance.SetIngredient(ingredient, true);
                break;
            case DropZone.RightPotion:
                MixPotionMode.Instance.SetIngredient(ingredient, false);
                break; 
            case DropZone.MiddlePotion:
                if (MixPotionMode.Instance.IsPossibleToSetLeft())
                {
                    MixPotionMode.Instance.SetIngredient(ingredient, true);
                }
                else MixPotionMode.Instance.SetIngredient(ingredient, false);
                break;
            case DropZone.LeftDish:
                MixDishMode.Instance.AddLeftIngredient(ingredient);
                break;
            case DropZone.RightDish:
                MixDishMode.Instance.AddRightIngredient(ingredient);
                break;
            case DropZone.MiddleDish:
                MixDishMode.Instance.SetCatalystIngredient(ingredient);
                break;
        }
    }
}