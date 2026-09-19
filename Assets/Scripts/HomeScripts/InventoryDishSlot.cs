using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class InventoryDishSlot : MonoBehaviour, IPointerClickHandler, IDropHandler
{
    public Image icon;
    public Button eatButton;

    private string id;

    public void Setup()
    {
        icon.color = new Color(1f, 1f, 1f, 0f);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ClearSlot();
    }

    public void OnDrop(PointerEventData eventData)
    {
        var dragged = eventData.pointerDrag?.GetComponent<StorageSlot>();
        if (dragged == null || dragged.slotData == null) return;
        SetIngredient(dragged.slotData);
    }

    public void SetIngredient(StorageContentData ingredient)
    {
        if (ingredient.count == 0)
        {
            ShakeButton.Instance.Shake(icon.transform.parent, "Количество равно нулю!");
            return;
        }
        icon.color = new Color(1f, 1f, 1f, 1f);
        icon.sprite = ingredient.icon;
        id = ingredient.data.id;
        eatButton.interactable = true;
    }

    public void ClearSlot() //NEED TO ADD METHOD TO eatButton
    {
        icon.sprite = null;
        eatButton.interactable = false;
        id = "";
    }
}
