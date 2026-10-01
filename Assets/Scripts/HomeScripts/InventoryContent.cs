using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System.Collections.Generic;

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

        List<InventorySaveData> savedKeySlots = InventoryWithKeysFactory.LoadBindings();
        foreach (var slotData in savedKeySlots)
        {
            InventorySlotWithKey slot = Instantiate(inventorySlotPrefab, contentParentPotions).AddComponent<InventorySlotWithKey>();
            bool available = slotData.slotIndex < 3;
            slot.Setup(slotData, available);
        }

        List<IngredientData> savedSlots = InventoryFactory.LoadGoods();
        foreach (var slotData in savedSlots)
        {
            InventorySlot slot = Instantiate(inventorySlotPrefab, contentParentInventory).AddComponent<InventorySlot>();
            slot.Setup(slotData);
        }
        int emptySlotsCount = 8 - savedSlots.Count;
        for (int i = 0; i < emptySlotsCount; i++)
        {
            InventorySlot slot = Instantiate(inventorySlotPrefab, contentParentInventory).AddComponent<InventorySlot>();
            slot.Setup(new IngredientData { id = "", count = 0 });
        }
    }
}
