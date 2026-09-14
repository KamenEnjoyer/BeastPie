using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class MenuAlchemy : MonoBehaviour
{
    public static MenuAlchemy Instance;
    public GameObject potionLongSlotPrefab;
    public Transform potionListContentParent;

    public GameObject smallSlotPrefab;
    public Transform effectsListContentParent;
    public Transform recipeContentParent;

    public GameObject potionInformation;
    public Image icon;
    public TMP_InputField nameText;
    public TMP_InputField descriptionText;
    public TMP_InputField effectText;

    public GameObject bottomPanel;

    public Button mixButton;

    private GILData potionData;

    void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        nameText.onEndEdit.AddListener(delegate { UpdateName(nameText); });
        descriptionText.onEndEdit.AddListener(delegate { UpdateDescription(descriptionText); });
        effectText.onEndEdit.AddListener(delegate { UpdateEffect(effectText); });
        mixButton.GetComponentInChildren<TextMeshProUGUI>().text = LocalizationSettings.StringDatabase.GetLocalizedString("UILocalizationStrings", "brew_potion") + " (0)";
        potionInformation.SetActive(false);

        SetPotionsList();
    }

    public void SetPotionsList()
    {
        List<GILData> potions = GILFactory.LoadIngredients();
        foreach (Transform child in potionListContentParent) Destroy(child.gameObject);
        foreach (var slotData in potions)
        {
            if (slotData.type == GILData.IngredientType.Potion)
            {
                MenuAlchemyLongSlot slot = Instantiate(potionLongSlotPrefab, potionListContentParent, false).GetComponent<MenuAlchemyLongSlot>();
                slot.Setup(slotData);
            }
        }
    }

    public void ShowPotionInformation(GILData data)
    {
        potionData = data;
        potionInformation.SetActive(true);
        Canvas.ForceUpdateCanvases();

        float panelHeight = bottomPanel.GetComponent<RectTransform>().rect.height - 30f;
        mixButton.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, panelHeight * 0.1f);
        descriptionText.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, panelHeight * 0.25f);
        effectText.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, panelHeight * 0.25f);
        effectsListContentParent.GetComponent<GridLayoutGroup>().cellSize = new Vector2(panelHeight * 0.2f, panelHeight * 0.2f);
        recipeContentParent.GetComponent<GridLayoutGroup>().cellSize = new Vector2(panelHeight * 0.2f, panelHeight * 0.2f);
        effectsListContentParent.GetComponentInParent<ScrollRect>().GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, panelHeight * 0.2f);
        recipeContentParent.GetComponentInParent<ScrollRect>().GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, panelHeight * 0.2f);

        icon.sprite = Resources.Load<Sprite>("IngredientsSprites/default");
        nameText.text = data.ingName;
        descriptionText.text = data.description;
        effectText.text = data.effect;
        mixButton.GetComponentInChildren<TextMeshProUGUI>().text = Regex.Replace(mixButton.GetComponentInChildren<TextMeshProUGUI>().text, @"\([^)]*\)", "(" + IngredientFactory.GetCountById(data.id) + ")");


        nameText.ForceLabelUpdate();
        descriptionText.ForceLabelUpdate();
        effectText.ForceLabelUpdate();

        foreach (Transform child in effectsListContentParent) Destroy(child.gameObject);
        foreach (var effectId in data.effectIds)
        {
            Debug.Log("Showing effect information for " + effectId);
            MenuSmallSlot slot = Instantiate(smallSlotPrefab, effectsListContentParent).GetComponent<MenuSmallSlot>();
            slot.Setup(Resources.Load<Sprite>("EffectsSprites/" + effectId), "");
        }

        foreach (Transform child in recipeContentParent) Destroy(child.gameObject);
        foreach (var ingredient in data.recipe)
        {
            MenuSmallSlot slot = Instantiate(smallSlotPrefab, recipeContentParent).GetComponent<MenuSmallSlot>();
            string ingSprite = ingredient.id.Substring(0, ingredient.id.Length - 2);
            slot.Setup(Resources.Load<Sprite>("IngredientsSprites/" + ingSprite), "x" + ingredient.count.ToString());
        }
    }

    public void MixPotion() 
    {
        foreach (var ingredient in potionData.recipe)
        {
            if (IngredientFactory.GetCountById(ingredient.id) < ingredient.count)
            {
                ShakeButton.Instance.Shake(mixButton.transform, "", false);
                return;
            }
        }

        foreach (var ingredient in potionData.recipe)
        {
            IngredientFactory.RemoveIngredient(ingredient.id, ingredient.count, ScenesConfig.IsHome);
        }
        string homeOrCamp = ScenesConfig.IsHome ? "Home" : "Camp";
        IngredientFactory.AddIngredient(potionData.id, 1, homeOrCamp);
        string newText = Regex.Replace(mixButton.GetComponentInChildren<TextMeshProUGUI>().text, @"\([^)]*\)", "(" + IngredientFactory.GetCountById(potionData.id) + ")");
        mixButton.GetComponentInChildren<TextMeshProUGUI>().text = newText;
    }

    public void UpdateName(TMP_InputField input)
    {
        potionData.ingName = input.text;
        GILFactory.UpdatePotionData(potionData);
        SetPotionsList();
    }

    public void UpdateDescription(TMP_InputField input)
    {
        potionData.description = input.text;
        GILFactory.UpdatePotionData(potionData);
    }

    public void UpdateEffect(TMP_InputField input)
    {
        potionData.effect = input.text;
        GILFactory.UpdatePotionData(potionData);
    }
}
