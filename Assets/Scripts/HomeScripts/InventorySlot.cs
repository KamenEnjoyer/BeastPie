using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IPointerClickHandler, IDropHandler
{
    public Image icon;
    public TextMeshProUGUI countText;
    private IngredientData slotData;

    public void Setup(IngredientData data)
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

        if (data.count > 0) countText.text = data.count.ToString();
        else countText.text = "";

        icon.color = new Color(1f, 1f, 1f, 1f);
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
        if (ingredient.data.id == "water" || ingredient.data.id == "fire")
        {
            ShakeButton.Instance.Shake(icon.GetComponentInParent<Button>().transform, "Нет нужны брать с собой.");
            return;
        }
        if (ingredient.count == 0)
        {
            ShakeButton.Instance.Shake(icon.GetComponentInParent<Button>().transform, "Количество равно нулю!");
            return;
        }
        icon.sprite = ingredient.icon;
        slotData.id = ingredient.data.id;
        countText.text = ingredient.count.ToString();
        GetComponent<IngredientHover>()?.SetIngredient(ingredient);
        InventoryFactory.AddNewIngredient(slotData);
    }

    public void ClearSlot()
    {
        icon.sprite = null;
        slotData.id = "";
        countText.text = "";
        GetComponent<IngredientHover>()?.SetIngredient(null);
        InventoryFactory.AddNewIngredient(slotData);
    }
}
