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

    private List<InventorySlot> inventorySlots = new List<InventorySlot>();
    private List<InventorySlotWithKey> potionSlots = new List<InventorySlotWithKey>();

    private void Awake()
    {
        Instance = this;
    }

    public void RefreshAllSlots()
    {
        foreach (Transform child in contentParentPotions) Destroy(child.gameObject);
        //potionSlots.Clear();
        foreach (Transform child in contentParentInventory) Destroy(child.gameObject);
        //inventorySlots.Clear();

        List<InventorySaveData> savedKeySlots = InventoryWithKeysFactory.LoadBindings();
        foreach (var slotData in savedKeySlots)
        {
            InventorySlotWithKey slot = Instantiate(inventorySlotPrefab, contentParentPotions).GetComponent<InventorySlotWithKey>();
            bool available = slotData.slotIndex < 3;
            slot.Setup(slotData, available);
            Destroy(slot.GetComponent<InventorySlot>());
            //potionSlots.Add(slot);
        }

        List<IngredientData> savedSlots = InventoryFactory.LoadGoods();
        foreach (var slotData in savedSlots)
        {
            InventorySlot slot = Instantiate(inventorySlotPrefab, contentParentInventory).GetComponent<InventorySlot>();
            slot.Setup(slotData);
            Destroy(slot.GetComponent<InventorySlotWithKey>());
            //inventorySlots.Add(slot);
        }
        int emptySlotsCount = 8 - savedSlots.Count;
        for (int i = 0; i < emptySlotsCount; i++)
        {
            InventorySlot slot = Instantiate(inventorySlotPrefab, contentParentInventory).GetComponent<InventorySlot>();
            slot.Setup(new IngredientData { id = "", count = 0 });
            Destroy(slot.GetComponent<InventorySlotWithKey>());
            //inventorySlots.Add(slot);
        }
    }
}
