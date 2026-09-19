using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerClickHandler, IDropHandler
{
    public Image icon;
    public TextMeshProUGUI nameText;
    private InventorySaveData slotData;

    private bool isAvailable = true;

    public void Setup(InventorySaveData data, bool available)
    {
        slotData = data;
        isAvailable = available;

        if (data.ingredientId == "")
        {
            icon.sprite = null;
            nameText.text = data.key.Replace("Alpha", ""); 
        }
        else
        {
            StorageContentData ingredient = StorageContent.Instance.GetIngredientById(data.ingredientId);
            icon.sprite = ingredient.icon;
            nameText.text = data.key.Replace("Alpha", "") + " — " + ingredient.data.ingName;
            GetComponent<IngredientHover>()?.SetIngredient(ingredient);
        }

        if (!available)
        {
            icon.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
            nameText.text = "";
        }
        else icon.color = new Color(1f, 1f, 1f, 1f);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isAvailable)
        {
            ShakeButton.Instance.Shake(icon.GetComponentInParent<Button>().transform, "Хотите купить ячейку?", true); //Ну, пока что хотеть не вредно
            return;
        }
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
        if (!isAvailable)
        {
            ShakeButton.Instance.Shake(icon.GetComponentInParent<Button>().GetComponentInParent<Button>().transform, "Требуется купить ячейку.");
            return;
        }
        if (ingredient.data.id == "water" || ingredient.data.id == "fire")
        {
            ShakeButton.Instance.Shake(icon.GetComponentInParent<Button>().transform, "Нет возможности использовать в бою.");
            return;
        }
        if (ingredient.count == 0)
        {
            ShakeButton.Instance.Shake(icon.GetComponentInParent<Button>().transform, "ВНИМАНИЕ! Количество равно нулю!");
        }
        icon.sprite = ingredient.icon;
        nameText.text = slotData.key.Replace("Alpha", "") + " — " + ingredient.data.ingName;
        slotData.ingredientId = ingredient.data.id;
        InventoryFactory.SaveNamesByIngredient(slotData);
    }   

    public void ClearSlot()
    {
        icon.sprite = null;
        nameText.text = slotData.key.Replace("Alpha", "");
        slotData.ingredientId = "";
        InventoryFactory.SaveNamesByIngredient(slotData);
    }
}
