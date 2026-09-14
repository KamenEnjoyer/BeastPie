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
        icon.sprite = Resources.Load<Sprite>("IngredientsSprites/" + id.Substring(0, id.Length - 2));
        nameText.text = LocalizationSettings.StringDatabase.GetLocalizedString("IngredientsNamesLocalization", id.Substring(0, id.Length - 2)) + " (" + count.ToString() + ")";
    }
}
