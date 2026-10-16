using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryPlayerSlot : InventorySlotType
{
    private int slotIndex;
    private bool isAvailable = true;

    public override void Setup(IngredientData data, int index, bool available)
    {
        slotIndex = index;
        isAvailable = available;

        PlayerInput playerInput = FindAnyObjectByType<PlayerInput>();
        text.text = playerInput.actions.FindAction("UsePotion" + (slotIndex+1)).GetBindingDisplayString().Replace("Alpha", "");

        if (!available)
        {
            icon.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
            text.text = "";
        }
        else icon.color = new Color(1f, 1f, 1f, 1f);

        SetupIngredient(data);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (!isAvailable)
        {
            ShakeButton.Instance.Shake(icon.GetComponentInParent<Button>().transform, "Хотите купить ячейку?", true); //Ну, пока что хотеть не вредно
            return;
        }
        ClearSlot();
    }

    public override void SetIngredient(StorageContentData ingredient)
    {
        if (!isAvailable)
        {
            ShakeButton.Instance.Shake(icon.GetComponentInParent<Button>().GetComponentInParent<Button>().transform, "Требуется купить ячейку.");
            return;
        }
        if (ingredient.data.type != GILData.IngredientType.Potion && ingredient.data.type != GILData.IngredientType.Loot)
        {
            ShakeButton.Instance.Shake(icon.GetComponentInParent<Button>().transform, "Нет возможности использовать в бою.");
            return;
        }
        if (ingredient.count == 0)
        {
            ShakeButton.Instance.Shake(icon.GetComponentInParent<Button>().transform, "ВНИМАНИЕ! Количество равно нулю!");
        }

        icon.sprite = ingredient.icon;
        slotData.id = ingredient.data.id;
        slotData.count = ingredient.count;
        GetComponent<IngredientHover>()?.SetIngredient(ingredient);

        InventoryFactory.SaveDataByIndex(slotData, slotIndex, true);
    }  
    
    public int GetSlotIndex()
    {
        return slotIndex;
    }

    public override void ClearSlot()
    {
        icon.sprite = null;
        slotData.id = "";
        slotData.count = 0;
        GetComponent<IngredientHover>()?.SetIngredient(null);
        InventoryFactory.SaveDataByIndex(slotData, slotIndex, true);
    }
}
