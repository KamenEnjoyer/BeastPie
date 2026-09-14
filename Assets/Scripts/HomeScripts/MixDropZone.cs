using UnityEngine;

public class MixDropZone : MonoBehaviour
{
    public enum DropZone { Left, Right, Middle }
    public DropZone zone;

    public void OnIngredientDropped(StorageContentData ingredient)
    {
        if (zone == DropZone.Left) MixPotionMode.Instance.SetIngredient(ingredient, true);
        else if (zone == DropZone.Right) MixPotionMode.Instance.SetIngredient(ingredient, false);
        else if (zone == DropZone.Middle) 
        {
            if (MixPotionMode.Instance.IsPossibleToSetLeft())
            {
                MixPotionMode.Instance.SetIngredient(ingredient, true);
            }
            else MixPotionMode.Instance.SetIngredient(ingredient, false);
        }
    }
}