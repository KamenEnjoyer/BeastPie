using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class LanguageButton : MonoBehaviour
{
    private Button button;
    private TMP_Text buttonText;

    private void Start()
    {
        SetLanguage("en");
    }

    private void Awake()
    {
        button = GetComponent<Button>();
        buttonText = GetComponentInChildren<TMP_Text>();

        button.onClick.AddListener(ToggleLanguage);
        //buttonText.text = "English";
    }

    private void ToggleLanguage()
    {
        switch (buttonText.text)
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
}
