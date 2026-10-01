using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotCanvasUpdate : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI nameText;

    void Start()
    {
        Canvas.ForceUpdateCanvases();
        float height = GetComponent<RectTransform>().rect.height - 10f;
        icon.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        float width = GetComponent<RectTransform>().rect.width - 15f - height;
        nameText.transform.parent.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        nameText.transform.parent.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        nameText.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, height);
        nameText.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, width);

    }
}
