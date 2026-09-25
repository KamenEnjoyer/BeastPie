using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

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

        List<InventorySaveData> savedSlots = InventoryFactory.LoadBindings();

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

    private void Update()
    {
        foreach (var slotData in potionSlotsData)
        {
            KeyCode key = (KeyCode)System.Enum.Parse(typeof(KeyCode), slotData.key);
            if (Input.GetKeyDown(key))
            {
                slots.FirstOrDefault(s => s.GetIngredientId() == slotData.ingredientId)?.ConsumeOne();
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
