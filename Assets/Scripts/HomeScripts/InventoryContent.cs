using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryContent : MonoBehaviour
{
    public static InventoryContent Instance;

    public Transform contentParent;
    public GameObject inventorySlotPrefab;

    private List<InventorySlot> slots = new List<InventorySlot>();

    private void Awake()
    {
        Instance = this;
    }

    public void RefreshAllSlots()
    {
        foreach (Transform child in contentParent) Destroy(child.gameObject);
        slots.Clear();

        List<InventorySaveData> savedSlots = InventoryFactory.LoadBindings();

        foreach (var slotData in savedSlots)
        {
            InventorySlot slot = Instantiate(inventorySlotPrefab, contentParent).GetComponent<InventorySlot>();
            bool available = slotData.slotIndex < 3;
            slot.Setup(slotData, available);
            slots.Add(slot);
        }
    }
}
