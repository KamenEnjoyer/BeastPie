using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryKeysListener : MonoBehaviour
{
    public Transform contentParent;

    public void OnUsePotion1()
    {
        SetHoveredSlot(0);
    }

    public void OnUsePotion2()
    {
        SetHoveredSlot(1);
    }

    public void OnUsePotion3()
    {
        SetHoveredSlot(2);
    }

    public void OnUsePotion4()
    {
        SetHoveredSlot(3);
    }

    public void OnUsePotion5()
    {
        SetHoveredSlot(4);
    }

    public void OnUsePotion6()
    {
        SetHoveredSlot(5);
    }

    public void OnUsePotion7()
    {
        SetHoveredSlot(6);
    }

    public void OnUsePotion8()
    {
        SetHoveredSlot(7);
    }

    private void SetHoveredSlot(int potionIndex)
    {
        var hoveredSlot = StorageSlot.GetHoveredSlot();
        if (hoveredSlot == null || hoveredSlot.slotData == null) return;

        foreach (Transform child in contentParent)
        {
            int slotId = child.GetComponent<InventorySlotWithKey>().GetSlotIndex();
            if (slotId == potionIndex)
            {
                child.GetComponent<InventorySlotWithKey>().SetIngredient(hoveredSlot.slotData);
                return;
            }
        }
    }
}