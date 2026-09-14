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
        Debug.Log("Small slot setup with icon: " + iconSprite.name);
        count.text = countText;
    }
}
