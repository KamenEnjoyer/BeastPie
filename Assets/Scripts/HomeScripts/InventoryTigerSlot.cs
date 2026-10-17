using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryTigerSlot : InventorySlotType
{
    private int slotIndex;

    public override void Setup(IngredientData data, int index, bool available = true)
    {
        slotIndex = index;

        if (data.count > 0) text.text = data.count.ToString();
        else text.text = "";

        icon.color = new Color(1f, 1f, 1f, 1f);

        SetupIngredient(data);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        ClearStart();
    }

    public override void SetIngredient(StorageContentData ingredient)
    {
        if (ingredient.data.id == "water" || ingredient.data.id == "fire")
        {
            ShakeButton.Instance.Shake(icon.GetComponentInParent<Button>().transform, "Нет нужны брать с собой.");
            return;
        }
        if (ingredient.count == 0)
        {
            ShakeButton.Instance.Shake(icon.GetComponentInParent<Button>().transform, "Количество равно нулю!");
            return;
        }

        if (slotData != null && slotData.id != "")
        {
            IngredientFactory.AddIngredient(slotData.id, slotData.count,
                        ScenesConfig.IsHome ? "Home" : "Camp");
        }

        icon.sprite = ingredient.icon;
        slotData.id = ingredient.data.id;
        slotData.count = ingredient.count;
        text.text = ingredient.count.ToString();
        GetComponent<IngredientHover>()?.SetIngredient(ingredient);

        InventoryFactory.SaveDataByIndex(slotData, slotIndex, false);
        IngredientFactory.RemoveIngredient(slotData.id, slotData.count, ScenesConfig.IsHome);

    }

    public override void ClearEnd()
    {
        text.text = "";
        InventoryFactory.SaveDataByIndex(slotData, slotIndex, false);
    }
}
