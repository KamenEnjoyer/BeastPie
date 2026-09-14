using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.Localization.Settings;

public class IngredientHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private StorageContentData ingredientData;

    public void SetIngredient(StorageContentData data)
    {
        ingredientData = data;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (ingredientData != null)
        {
            if (ingredientData.data.type == GILData.IngredientType.Catalyst)
            {
                Description.Instance.ShowDescription(ingredientData.data.ingName + "\n" + ingredientData.data.description);
                return;
            }
            string ingredientText = ingredientData.data.ingName + "\n";
            if(StorageMarketPanel.Instance.IsMarketPanelActive())
            {
                ingredientText += LocalizationSettings.StringDatabase.GetLocalizedString("UILocalizationStrings", "price") + ": " + ingredientData.data.price * (ingredientData.data.density+1) + "\n";
            }
            if (ingredientData.data.type != GILData.IngredientType.Food)
            {
                ingredientText +=
                LocalizationSettings.StringDatabase.GetLocalizedString("UILocalizationStrings", "density") + ": " + ingredientData.data.density + " | " +
                LocalizationSettings.StringDatabase.GetLocalizedString("UILocalizationStrings", "density_limit") + ": " + ingredientData.data.densityLimit + "\n";
            }
            ingredientText += ingredientData.data.description;
            Description.Instance.ShowDescription(ingredientText);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Description.Instance.ClearDescription();
    }
}
