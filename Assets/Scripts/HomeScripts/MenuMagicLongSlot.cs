using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class MenuMagicLongSlot : MonoBehaviour, IPointerClickHandler
{
    public Image icon;
    public TextMeshProUGUI nameText;

    private string effectId;

    public void Setup(string id)
    {
        effectId = id;

        if (Resources.Load<Sprite>("EffectsSprites/" + id) == null) icon.sprite = Resources.Load<Sprite>("EffectsSprites/default");
        else icon.sprite = Resources.Load<Sprite>("EffectsSprites/" + id);
        nameText.text = LocalizationSettings.StringDatabase.GetLocalizedString("EffectNamesLocalization", id);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (effectId == null) return;
        MenuMagic.Instance?.ShowEffectInformation(effectId);
    }
}
