using UnityEngine;

public class InventoryContent : MonoBehaviour
{
    public static InventoryContent Instance;

    public Transform contentParentPotions;
    public Transform contentParentInventory;
    public GameObject inventorySlotPrefab;

    private void Awake()
    {
        Instance = this;
    }

    public void RefreshAllSlots()
    {
        foreach (Transform child in contentParentPotions) Destroy(child.gameObject);
        foreach (Transform child in contentParentInventory) Destroy(child.gameObject);

        foreach (var invData in InventoryFactory.LoadGoods(true))
        {
            InventoryPlayerSlot slot = Instantiate(inventorySlotPrefab, contentParentPotions).AddComponent<InventoryPlayerSlot>();
            bool available = invData.slotIndex < 3;
            slot.Setup(invData.slotData, invData.slotIndex, available);
        }

        foreach (var invData in InventoryFactory.LoadGoods(false))
        {
            InventoryTigerSlot slot = Instantiate(inventorySlotPrefab, contentParentInventory).AddComponent<InventoryTigerSlot>();
            slot.Setup(invData.slotData, invData.slotIndex, true);
        }
    }
}
