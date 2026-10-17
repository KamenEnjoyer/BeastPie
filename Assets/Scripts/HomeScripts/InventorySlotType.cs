using TMPro;
using UnityEditor.Experimental.GraphView;
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
        dragged.DestroyDragIcon();
        SetIngredient(dragged.slotData);
    }

    public abstract void SetIngredient(StorageContentData ingredient);

    public void ClearStart()
    {
        GetComponent<IngredientHover>()?.SetIngredient(null);
        IngredientFactory.AddIngredient(slotData.id, slotData.count, 
            ScenesConfig.IsHome ? "Home" : "Camp");

        icon.sprite = null;
        slotData.id = "";
        slotData.count = 0;

        ClearEnd();
    }

    public abstract void ClearEnd();
}
