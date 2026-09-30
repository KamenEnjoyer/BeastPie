using System.Collections.Generic;
using UnityEngine;

public class InventoryInfo : MonoBehaviour
{
    public static InventoryInfo Instance;

    public Transform potionsContentParent;
    public Transform inventoryContentParent;
    public Transform effectsContentParent;
    public GameObject inventorySlotPrefab;

    private List<MenuSmallSlot> potionSlots = new List<MenuSmallSlot>();
    private List<InventorySlot> inventorySlots = new List<InventorySlot>();
    private List<InventorySlot> effectsSlots = new List<InventorySlot>();

    private void Awake()
    {
        Instance = this;
    }

    /*public void RefreshAllSlots()
    {
        foreach (Transform child in potionsContentParent) Destroy(child.gameObject);
        

        List<InventorySaveData> savedSlots = InventoryFactory.LoadBindings();

        foreach (var slotData in savedSlots)
        {
            InventorySlot slot = Instantiate(inventorySlotPrefab, contentParent).GetComponent<InventorySlot>();
            bool available = slotData.slotIndex < 3;
            slot.Setup(slotData, available);
            slots.Add(slot);
        }
    }*/
}
