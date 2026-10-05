using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
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
        SetLanguage("en");
    }

    private void Awake()
    {
        //buttonText.text = "English";
    }

    public void ToggleLanguage()
    {
        switch (languageButton.GetComponentInChildren<TMP_Text>().text)
        {
            case "English":
                SetLanguage("ru");
                RefreshAllIngreients();
                break;
            case "Русский":
                SetLanguage("lt");
                RefreshAllIngreients();
                break;
            case "Lietuvių":
                SetLanguage("en");
                RefreshAllIngreients();
                break;
        }
    }

    public void SetLanguage(string languageCode)
    {
        Locale locale = LocalizationSettings.AvailableLocales.GetLocale(languageCode);
        if (locale != null)
        {
            LocalizationSettings.SelectedLocale = locale;
        }
    }

    public void RefreshAllIngreients()
    {
        GILFactory.UpdateLocalization();
        StorageContent.Instance.UpdateIngredients();
        InventoryContent.Instance?.RefreshAllSlots();
        //REFACTOR
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
