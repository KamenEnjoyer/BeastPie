using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuButtons : MonoBehaviour
{
    public Button languageButton;

    private void Start()
    {
        Canvas.ForceUpdateCanvases();
        foreach (Transform child in transform)
        {
            float width = transform.GetComponent<RectTransform>().rect.width * 0.8f;
            child.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
            child.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, width*0.2f);
        }
    }

    public void ToggleLanguage()
    {
        switch (ScenesConfig.currentLanguage)
        {
            case ScenesConfig.languageVariables.en:
                SetLanguage(ScenesConfig.languageVariables.ru);
                break;
            case ScenesConfig.languageVariables.ru:
                SetLanguage(ScenesConfig.languageVariables.lt);
                break;
            case ScenesConfig.languageVariables.lt:
                SetLanguage(ScenesConfig.languageVariables.en);
                break;
        }
        RefreshAllIngreients();
        ConfigFactory.SaveConfig();
    }

    public void SetLanguage(ScenesConfig.languageVariables languageCode)
    {
        ScenesConfig.currentLanguage = languageCode;
        Locale locale = LocalizationSettings.AvailableLocales.GetLocale(languageCode.ToString());
        if (locale != null)
        {
            LocalizationSettings.SelectedLocale = locale;
        }
    }

    public void RefreshAllIngreients()
    {
        GILFactory.UpdateLocalization();

        if (SceneManager.GetActiveScene().name == "HomeScene")
        {
            StorageContent.Instance?.UpdateIngredients();
            InventoryContent.Instance?.RefreshAllSlots();
        }
        else if (SceneManager.GetActiveScene().name == "CombatScene")
        {
            
        }
        else if (SceneManager.GetActiveScene().name == "MapScene")
        {
            
        }
        //REFACTOR
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
