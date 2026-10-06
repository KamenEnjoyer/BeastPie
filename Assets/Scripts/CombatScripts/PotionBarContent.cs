using System.Collections.Generic;
using UnityEngine;

public class PotionBarContent : MonoBehaviour
{
    public static PotionBarContent Instance;

    public Transform contentParent;
    public GameObject potionSlotPrefab;

    private List<PotionBarSlot> slots = new List<PotionBarSlot>();
    private List<InventorySaveData> potionSlotsData = new List<InventorySaveData>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        foreach (Transform child in contentParent) Destroy(child.gameObject);
        slots.Clear();

        List<InventorySaveData> savedSlots = InventoryWithKeysFactory.LoadBindings();

        bool isEmpty = true;
        foreach (var slotData in savedSlots)
        {
            if (slotData.ingredientId != "")
            {
                potionSlotsData.Add(slotData);
                PotionBarSlot slot = Instantiate(potionSlotPrefab, contentParent).GetComponent<PotionBarSlot>();
                slot.Setup(slotData);
                slots.Add(slot);
                isEmpty = false;
            }
        }

        if (isEmpty) gameObject.SetActive(false);
    }

    public void OnUsePotion1()
    {
        UsePotion(0);
    }

    public void OnUsePotion2()
    {
        UsePotion(1);
    }

    public void OnUsePotion3()
    {
        UsePotion(2);
    }

    public void OnUsePotion4()
    {
        UsePotion(3);
    }

    public void OnUsePotion5()
    {
        UsePotion(4);
    }

    public void OnUsePotion6()
    {
        UsePotion(5);
    }

    public void OnUsePotion7()
    {
        UsePotion(6);
    }

    public void OnUsePotion8()
    {
        UsePotion(7);
    }

    private void UsePotion(int index)
    {
        foreach (var slot in slots)
        {
            if (slot.index == index)
            {
                slot.ConsumeOne();
                return;
            }
        }
    }

    public void SaveNewPotionCounts()
    {
        foreach (var potionSlot in slots)
        {
            potionSlot.SaveNewPotionCount();
        }
    }
}
