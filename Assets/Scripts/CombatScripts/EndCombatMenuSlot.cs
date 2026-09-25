using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class EndCombatMenuSlot : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI nameText;

    public void Setup(string id, int count)
    {
        icon.sprite = Resources.Load<Sprite>("IngredientsSprites/" + id);
        nameText.text = LocalizationSettings.StringDatabase.GetLocalizedString("IngredientsNamesLocalization", id) + " (" + count.ToString() + ")";
    }
}
