using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class InventorySlotType : MonoBehaviour, IPointerClickHandler, IDropHandler
{
    protected Image icon;
    protected TextMeshProUGUI text;
    protected IngredientData slotData;

    void Awake()
    {
        icon = gameObject.GetComponent<InventorySlot>().GetIcon();
        text = gameObject.GetComponent<InventorySlot>().GetNameText();
    }

    public abstract void Setup(IngredientData data, int index, bool available);

    public void SetupIngredient(IngredientData data)
    {
        slotData = data;

        if (data.id == "")
        {
            icon.sprite = null;
        }
        else
        {
            StorageContentData ingredient = StorageContent.Instance.GetIngredientById(data.id);
            icon.sprite = ingredient.icon;
            GetComponent<IngredientHover>()?.SetIngredient(ingredient);
        }
    }

    public abstract void OnPointerClick(PointerEventData eventData);

    public void OnDrop(PointerEventData eventData)
    {
        var dragged = eventData.pointerDrag?.GetComponent<StorageSlot>();
        if (dragged == null || dragged.slotData == null) return;
        SetIngredient(dragged.slotData);
    }

    public abstract void SetIngredient(StorageContentData ingredient);

    public abstract void ClearSlot();
}
