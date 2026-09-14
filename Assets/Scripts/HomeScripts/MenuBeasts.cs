using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.U2D;
using UnityEngine.UI;

public class MenuBeasts : MonoBehaviour
{
    public static MenuBeasts Instance;

    public GameObject beastLongSlotPrefab;
    public Transform beastsListContentParent;

    public GameObject beastInformation;
    public GameObject topPanel;
    public GameObject bottomPanel;
    public GameObject bottomPanelTopSection;
    public Image icon;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI health;
    public TextMeshProUGUI type;
    public TextMeshProUGUI killCount;
    public TextMeshProUGUI descriptionText;

    public GameObject smallSlotPrefab;
    public Transform locationsListContentParent;
    public Transform lootContentParent;

    void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        beastInformation.SetActive(false);

        SetBeastsList();
    }

    public void SetBeastsList()
    {
        List<EnemySaveData> beasts = EnemyFactory.LoadBeasts();
        foreach (Transform child in beastsListContentParent) Destroy(child.gameObject);
        foreach (var slotData in beasts)
        {
            MenuBeastsLongSlot slot = Instantiate(beastLongSlotPrefab, beastsListContentParent, false).GetComponent<MenuBeastsLongSlot>();
            slot.Setup(slotData);
        }
    }

    public void ShowBeastInformation(EnemySaveData data)
    {
        beastInformation.SetActive(true);
        Canvas.ForceUpdateCanvases();

        EnemyTypeAsset enemy = Resources.Load<EnemyTypeAsset>("EnemyTypes/" + data.id);

        float panelHeight = bottomPanel.GetComponent<RectTransform>().rect.height - 20f;
        bottomPanelTopSection.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, panelHeight * 0.6f);
        locationsListContentParent.GetComponentInParent<ScrollRect>().GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, panelHeight * 0.2f);
        lootContentParent.GetComponentInParent<ScrollRect>().GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, panelHeight * 0.2f);
        locationsListContentParent.GetComponent<GridLayoutGroup>().cellSize = new Vector2(panelHeight * 0.2f, panelHeight * 0.2f);
        lootContentParent.GetComponent<GridLayoutGroup>().cellSize = new Vector2(panelHeight * 0.2f, panelHeight * 0.2f);

        panelHeight = bottomPanelTopSection.GetComponent<RectTransform>().rect.height - 65f;
        descriptionText.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, panelHeight);

        float panelWeight = topPanel.GetComponent<RectTransform>().rect.width - 15f;
        nameText.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, panelWeight - icon.GetComponent<RectTransform>().rect.width);

        if (Resources.Load<Sprite>("EnemyIconSprites/" + enemy.id) == null) icon.sprite = Resources.Load<Sprite>("EnemyIconSprites/default");
        else icon.sprite = Resources.Load<Sprite>("EnemyIconSprites/" + enemy.id);
        nameText.text = LocalizationSettings.StringDatabase.GetLocalizedString("EnemyNamesLocalization", enemy.id);
        
        if(data.killCount > 8) descriptionText.text = LocalizationSettings.StringDatabase.GetLocalizedString("EnemyDescriptionsLocalization", enemy.id);
        else descriptionText.text = "???";

        if (enemy.emotionPattern == EnemyTypeAsset.EmotionPattern.None || data.killCount < 6) beastInformation.GetComponent<Image>().sprite = Resources.Load<Sprite>("Patterns/default");
        else beastInformation.GetComponent<Image>().sprite = Resources.Load<Sprite>("Patterns/" + enemy.emotionPattern.ToString());

        if (data.killCount > 11) health.text = LocalizationSettings.StringDatabase.GetLocalizedString("UILocalizationStrings", "health") + ": " + enemy.maxHealth.ToString();
        else health.text = LocalizationSettings.StringDatabase.GetLocalizedString("UILocalizationStrings", "health") + ": ???";

        if (data.killCount > 2) type.text = LocalizationSettings.StringDatabase.GetLocalizedString("EnemyTypeLocalization", enemy.behaviourType.ToString());
        else type.text = "???";

        killCount.text = LocalizationSettings.StringDatabase.GetLocalizedString("UILocalizationStrings", "kill_count") + data.killCount.ToString();

        foreach (Transform child in locationsListContentParent) Destroy(child.gameObject);
        foreach (var zoneName in data.habitats)
        {
            MenuSmallSlot slot = Instantiate(smallSlotPrefab, locationsListContentParent).GetComponent<MenuSmallSlot>();
            if (Resources.Load<Sprite>("LocationsSprites/" + zoneName) == null) slot.Setup(Resources.Load<Sprite>("LocationsSprites/default"), "");
            else slot.Setup(Resources.Load<Sprite>("LocationsSprites/" + zoneName), "");
        }

        foreach (Transform child in lootContentParent) Destroy(child.gameObject);
        foreach (var ingredient in enemy.lootTable)
        {
            MenuSmallSlot slot = Instantiate(smallSlotPrefab, lootContentParent).GetComponent<MenuSmallSlot>();
            string ingSprite = ingredient.ingredientId.Substring(0, ingredient.ingredientId.Length - 2);
            slot.Setup(Resources.Load<Sprite>("IngredientsSprites/" + ingSprite), ingredient.minQuantity.ToString() + "-" + ingredient.maxQuantity.ToString());
        }
    }
}
