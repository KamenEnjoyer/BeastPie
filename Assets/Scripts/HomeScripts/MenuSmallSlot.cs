using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuSmallSlot : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI count;

    public void Setup(Sprite iconSprite, string countText)
    {
        icon.sprite = iconSprite;
        count.text = countText;
    }
}
