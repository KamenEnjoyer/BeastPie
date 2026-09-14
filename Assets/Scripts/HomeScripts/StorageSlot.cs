using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class StorageSlot : MonoBehaviour,
                        IPointerClickHandler,
                        IBeginDragHandler,
                        IDragHandler,
                        IEndDragHandler,
                        IPointerEnterHandler,
                        IPointerExitHandler
{
    public GameObject storageSlotPrefab;
    private Image icon;
    private Image textBackgound;
    private TextMeshProUGUI nameText;

    public StorageContentData slotData;

    private Image dragIconInstance;
    public enum ZoneType { Left, Right }
    public ZoneType zoneType;

    private static StorageSlot hoveredSlot;

    private void Awake()
    {
        icon = storageSlotPrefab.transform.Find("Icon").GetComponent<Image>();
        nameText = GetComponentInChildren<TextMeshProUGUI>(true);
        textBackgound = storageSlotPrefab.transform.Find("Backgound").GetComponent<Image>();
    }

    public void Setup(StorageContentData data)
    {
        slotData = data;
        icon.sprite = slotData.icon;

        if (slotData.data.type == GILData.IngredientType.Potion || slotData.data.type == GILData.IngredientType.Loot)
        {
            float red, green, blue;
            if  (slotData.data.density < 6)
            {
                red = 0.5f - slotData.data.density * 0.04f;
                blue = slotData.data.density * 0.09f + 0.2f;
                green = 1f - slotData.data.density * 0.06f - 0.2f;
            }
            else
            {
                red = slotData.data.density * 0.045f;
                blue = 0.65f + slotData.data.density * 0.015f;
                green = 0.5f - slotData.data.density * 0.02f;
            }
            textBackgound.color = new Color(red, green, blue, 0.9f);
        }

        if (slotData.data.ingName.Length > 15) nameText.text = $"{slotData.data.ingName.Substring(0, 15)}... ({slotData.count})";
        else if (slotData.data.id == "water" || slotData.data.id == "fire") nameText.text = slotData.data.ingName;
        else nameText.text = $"{slotData.data.ingName} ({slotData.count})";

        if (slotData.count != 0) icon.color = new Color(1f, 1f, 1f, 1f);
        else icon.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);

        GetComponent<IngredientHover>()?.SetIngredient(data);
    }

    public void OnPointerEnter(PointerEventData eventData) => hoveredSlot = this;
    public void OnPointerExit(PointerEventData eventData) => hoveredSlot = null;
    public static StorageSlot GetHoveredSlot() => hoveredSlot;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (slotData == null) return;

        if(StorageMarketPanel.Instance.IsMarketPanelActive()) 
        {
            if (slotData.data.type != GILData.IngredientType.Catalyst)
            {
                if (GetComponentInParent<StorageMarket>() != null) StorageMarket.Instance.SetIngredientToTrade(slotData, true);
                if (GetComponentInParent<StorageContent>() != null) StorageMarket.Instance.SetIngredientToTrade(slotData, false);
            }
            else
            {
                Description.Instance.ShowMessage(slotData.data.ingName + " cannot be sold.");
            }
        }
        else
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                MixPotionMode.Instance?.SetIngredient(slotData, true);
            }
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                MixPotionMode.Instance?.SetIngredient(slotData, false);
            }
        }
        
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (slotData == null) return;

        dragIconInstance = new GameObject("DragIcon").AddComponent<Image>();
        dragIconInstance.transform.SetParent(transform.root, false);
        dragIconInstance.sprite = icon.sprite;
        dragIconInstance.raycastTarget = false;
        dragIconInstance.rectTransform.sizeDelta = icon.rectTransform.sizeDelta;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (dragIconInstance != null) dragIconInstance.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (dragIconInstance != null) Destroy(dragIconInstance.gameObject);

        var results = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        foreach (var result in results)
        {
            var dropZone = result.gameObject.GetComponent<MixDropZone>();
            if (dropZone != null)
            {
                dropZone.OnIngredientDropped(slotData);
                break;
            }
        }
    }
}
