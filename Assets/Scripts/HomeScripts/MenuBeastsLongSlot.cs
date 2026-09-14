using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class MenuBeastsLongSlot : MonoBehaviour, IPointerClickHandler
{
    public Image icon;
    public TextMeshProUGUI nameText;

    private EnemySaveData enemyType;

    public void Setup(EnemySaveData type)
    {
        enemyType = type;

        if(Resources.Load<Sprite>("EnemyIconSprites/" + enemyType.id) == null) icon.sprite = Resources.Load<Sprite>("EnemyIconSprites/default");
        else icon.sprite = Resources.Load<Sprite>("EnemyIconSprites/" + enemyType.id);
        nameText.text = LocalizationSettings.StringDatabase.GetLocalizedString("EnemyNamesLocalization", enemyType.id);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (enemyType == null) return;
        MenuBeasts.Instance?.ShowBeastInformation(enemyType);
    }
}
