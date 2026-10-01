using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class ZoneDescription : MonoBehaviour
{
    public TextMeshProUGUI zoneName;
    public Image icon;
    public TextMeshProUGUI description;

    public Transform enemiesListContent;
    public GameObject enemySlotPref;

    public Button goToCombatButton;

    public static ZoneDescription Instance;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Clear();
        Canvas.ForceUpdateCanvases();
        float width = icon.transform.parent.GetComponent<RectTransform>().rect.width - icon.GetComponent<RectTransform>().rect.width - 15f;
        zoneName.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
    }

    public void Clear()
    {
        goToCombatButton.interactable = false;
        zoneName.text = "";
        icon.color = new Color(1f, 1f, 1f, 0f);
        description.text = "";
        foreach (Transform child in enemiesListContent) Destroy(child.gameObject);
    }

    public void SetInfo(ZoneType data, bool canMove, bool unlocked)
    {
        Clear();

        if (unlocked)
        {
            zoneName.text = LocalizationSettings.StringDatabase.GetLocalizedString("ZonesNamesLocalization", data.id);
            description.text = LocalizationSettings.StringDatabase.GetLocalizedString("ZonesDescriptionLocalization", data.id);
            icon.color = new Color(1f, 1f, 1f, 1f);
            icon.sprite = Resources.Load<Sprite>("ZonesSprites/" + data.id);

            foreach (var enemy in data.enemies)
            {
                ZoneEnemySlot slot = Instantiate(enemySlotPref, enemiesListContent).GetComponent<ZoneEnemySlot>();
                slot.Setup(enemy.enemyType);
            }
        }
        else
        {
            zoneName.text = "???";
            icon.color = new Color(1f, 1f, 1f, 1f);
            icon.sprite = Resources.Load<Sprite>("ZonesSprites/default");
        }

        if (canMove) goToCombatButton.interactable = true;
    }
}
