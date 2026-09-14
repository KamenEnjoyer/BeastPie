using UnityEngine;
using System.Collections.Generic;

public class InventoryKeysListener : MonoBehaviour
{
    List<InventorySaveData> keyBindings;

    private void Start()
    {
        keyBindings = InventoryFactory.LoadBindings();
    }

    private void Update()
    {
        var hoveredSlot = StorageSlot.GetHoveredSlot();
        if (hoveredSlot == null || hoveredSlot.slotData == null) return;

        foreach (var inventorySlot in keyBindings)
        {
            KeyCode key = (KeyCode)System.Enum.Parse(typeof(KeyCode), inventorySlot.key);
            if (Input.GetKeyDown(key))
            {
                Debug.Log("Key: " + inventorySlot.key + "; Id: " + inventorySlot.slotIndex);
                var inventorySlots = FindObjectsByType<InventorySlot>();
                int index = inventorySlot.slotIndex;
                inventorySlots[inventorySlots.Length - index - 1].SetIngredient(hoveredSlot.slotData);
                return;
            }
        }
    }
}