using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class MenuMagic : MonoBehaviour
{
    public static MenuMagic Instance;

    public GameObject magicLongSlotPrefab;
    public Transform magicListContentParent;

    public GameObject effectInformation;

    public GameObject topPanel;
    public GameObject bottomPanel;

    public Image icon;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;

    public GameObject smallSlotPrefab;
    public Transform ingredientsContentParent;

    void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        effectInformation.SetActive(false);

        SetMagicList();
    }

    public void SetMagicList()
    {
        List<string> effects = new List<string>();

        foreach(GILData ing in GILFactory.LoadIngredients())
        {
            if (ing.type == GILData.IngredientType.Loot)
            {
                foreach (var effectId in ing.effectIds)
                {
                    if (!effects.Contains(effectId)) effects.Add(effectId);
                }
            }
        }

        foreach (Transform child in magicListContentParent) Destroy(child.gameObject);
        foreach (var effectId in effects)
        {
            MenuMagicLongSlot slot = Instantiate(magicLongSlotPrefab, magicListContentParent, false).GetComponent<MenuMagicLongSlot>();
            slot.Setup(effectId);
        }
    }

    public void ShowEffectInformation(string effectId)
    {
        effectInformation.SetActive(true);
        Canvas.ForceUpdateCanvases();

        float panelHeight = bottomPanel.GetComponent<RectTransform>().rect.height - 15f;
        descriptionText.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, panelHeight * 0.75f); ;
        ingredientsContentParent.GetComponentInParent<ScrollRect>().GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, panelHeight * 0.25f);
        ingredientsContentParent.GetComponent<GridLayoutGroup>().cellSize = new Vector2(panelHeight * 0.25f, panelHeight * 0.25f);

        float panelWeight = topPanel.GetComponent<RectTransform>().rect.width - 15f;
        nameText.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, panelWeight - icon.GetComponent<RectTransform>().rect.width);

        if (Resources.Load<Sprite>("EffectsSprites/" + effectId) == null) icon.sprite = Resources.Load<Sprite>("EffectsSprites/default");
        else icon.sprite = Resources.Load<Sprite>("EffectsSprites/" + effectId);
        nameText.text = LocalizationSettings.StringDatabase.GetLocalizedString("EffectNamesLocalization", effectId);

        descriptionText.text = LocalizationSettings.StringDatabase.GetLocalizedString("EffectDescriptionsLocalization", effectId);

        List<GILData> ingredientsData = new List<GILData>();
        foreach (GILData ing in GILFactory.LoadIngredients())
        {
            if (ing.type == GILData.IngredientType.Loot)
            {
                foreach (string effId in ing.effectIds)
                {
                    if (effId == effectId) ingredientsData.Add(ing);
                    break;
                }
            }
        }
        foreach (Transform child in ingredientsContentParent) Destroy(child.gameObject);
        foreach (var ingredient in ingredientsData)
        {
            MenuSmallSlot slot = Instantiate(smallSlotPrefab, ingredientsContentParent).GetComponent<MenuSmallSlot>();
            string ingSprite = ingredient.id.Substring(0, ingredient.id.Length - 2);
            slot.Setup(Resources.Load<Sprite>("IngredientsSprites/" + ingSprite), "");
        }
    }
}
