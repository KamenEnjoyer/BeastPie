using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InventoryInfo : MonoBehaviour
{
    public static InventoryInfo Instance;

    public Transform potionsContentParent;
    public Transform inventoryContentParent;
    public Transform effectsContentParent;
    public GameObject inventorySlotPrefab;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Canvas.ForceUpdateCanvases();
        float height = potionsContentParent.GetComponentInParent<ScrollRect>().GetComponent<RectTransform>().rect.height;
        potionsContentParent.GetComponent<GridLayoutGroup>().cellSize = new Vector2(height * 0.6f, height * 0.45f);
        inventoryContentParent.GetComponent<GridLayoutGroup>().cellSize = new Vector2(height * 0.6f, height * 0.45f);
        effectsContentParent.GetComponent<GridLayoutGroup>().cellSize = new Vector2(height * 0.6f, height * 0.45f);

        SetAllSlots();
    }

    public void SetAllSlots()
    {
        foreach (Transform child in potionsContentParent) Destroy(child.gameObject);
        foreach (Transform child in inventoryContentParent) Destroy(child.gameObject);

        SetInventorySlots(InventoryFactory.LoadGoods(true), potionsContentParent);
        SetInventorySlots(InventoryFactory.LoadGoods(false), inventoryContentParent);

        /*
        foreach (var invSlot in InventoryFactory.LoadGoods(true))
        {
            if (invSlot.slotData.id == null || invSlot.slotData.id == "") continue;
            InventorySlot slot = Instantiate(inventorySlotPrefab, potionsContentParent).GetComponent<InventorySlot>();
            
            Sprite icon;
            if (invSlot.slotData.id.IndexOf('_') < 0)
            {
                if (Resources.Load<Sprite>("IngredientsSprites/" + invSlot.slotData.id) != null) icon = Resources.Load<Sprite>("IngredientsSprites/" + invSlot.slotData.id);
                else icon = Resources.Load<Sprite>("IngredientsSprites/default");
            }
            else if (Resources.Load<Sprite>("IngredientsSprites/" + invSlot.slotData.id.Substring(0, invSlot.slotData.id.IndexOf('_'))) != null)
                icon = Resources.Load<Sprite>("IngredientsSprites/" + invSlot.slotData.id.Substring(0, invSlot.slotData.id.IndexOf('_')));
            else icon = Resources.Load<Sprite>("IngredientsSprites/default");

            slot.SetVisual(icon, invSlot.slotData.count.ToString());
        }

        foreach (var invSlot in InventoryFactory.LoadGoods(false))
        {
            if (invSlot.slotData.id == null || invSlot.slotData.id == "") continue;
            InventorySlot slot = Instantiate(inventorySlotPrefab, inventoryContentParent).GetComponent<InventorySlot>();

            Sprite icon;
            if (invSlot.slotData.id.IndexOf('_') < 0)
            {
                if (Resources.Load<Sprite>("IngredientsSprites/" + invSlot.slotData.id) != null) icon = Resources.Load<Sprite>("IngredientsSprites/" + invSlot.slotData.id);
                else icon = Resources.Load<Sprite>("IngredientsSprites/default");
            }
            else if (Resources.Load<Sprite>("IngredientsSprites/" + invSlot.slotData.id.Substring(0, invSlot.slotData.id.IndexOf('_'))) != null)
                icon = Resources.Load<Sprite>("IngredientsSprites/" + invSlot.slotData.id.Substring(0, invSlot.slotData.id.IndexOf('_')));
            else icon = Resources.Load<Sprite>("IngredientsSprites/default");

            slot.SetVisual(icon, invSlot.slotData.count.ToString());
        }*/

        List<EffectData> playerEffects = PlayerConfig.LoadDish();
        foreach (var effectData in playerEffects)
        {
            if (effectData.efcName == null || effectData.efcName == "") continue;
            InventorySlot slot = Instantiate(inventorySlotPrefab, effectsContentParent).GetComponent<InventorySlot>();
            Sprite icon;
            if (Resources.Load<Sprite>("EffectsSprites/" + effectData.efcName) != null) icon = Resources.Load<Sprite>("EffectsSprites/" + effectData.efcName);
            else icon = Resources.Load<Sprite>("EffectsSprites/default");
            slot.SetVisual(icon, effectData.screenCount.ToString());
        }
    }

    private void SetInventorySlots(List<InventoryFactory.InventoryData> inventoryData, Transform parent)
    {
        foreach (var invSlot in inventoryData)
        {
            if (invSlot.slotData.id == null || invSlot.slotData.id == "") continue;
            InventorySlot slot = Instantiate(inventorySlotPrefab, parent).GetComponent<InventorySlot>();

            Sprite icon;
            if (invSlot.slotData.id.IndexOf('_') < 0)
            {
                if (Resources.Load<Sprite>("IngredientsSprites/" + invSlot.slotData.id) != null) icon = Resources.Load<Sprite>("IngredientsSprites/" + invSlot.slotData.id);
                else icon = Resources.Load<Sprite>("IngredientsSprites/default");
            }
            else if (Resources.Load<Sprite>("IngredientsSprites/" + invSlot.slotData.id.Substring(0, invSlot.slotData.id.IndexOf('_'))) != null)
                icon = Resources.Load<Sprite>("IngredientsSprites/" + invSlot.slotData.id.Substring(0, invSlot.slotData.id.IndexOf('_')));
            else icon = Resources.Load<Sprite>("IngredientsSprites/default");

            slot.SetVisual(icon, invSlot.slotData.count.ToString());
        }
    }

    public void BackToHome()
    {
        SceneManager.LoadScene("LoadingScene");
    }
}
