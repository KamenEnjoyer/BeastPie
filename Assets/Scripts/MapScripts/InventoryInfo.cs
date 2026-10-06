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

        List<InventorySaveData> savedKeySlots = InventoryWithKeysFactory.LoadBindings();
        foreach (var slotData in savedKeySlots)
        {
            if (slotData.ingredientId == null || slotData.ingredientId == "") continue;
            InventorySlotAbstract slot = Instantiate(inventorySlotPrefab, potionsContentParent).GetComponent<InventorySlotAbstract>();
            
            Sprite icon;
            if (slotData.ingredientId.IndexOf('_') < 0)
            {
                if (Resources.Load<Sprite>("IngredientsSprites/" + slotData.ingredientId) != null) icon = Resources.Load<Sprite>("IngredientsSprites/" + slotData.ingredientId);
                else icon = Resources.Load<Sprite>("IngredientsSprites/default");
            }
            else if (Resources.Load<Sprite>("IngredientsSprites/" + slotData.ingredientId.Substring(0, slotData.ingredientId.IndexOf('_'))) != null)
                icon = Resources.Load<Sprite>("IngredientsSprites/" + slotData.ingredientId.Substring(0, slotData.ingredientId.IndexOf('_')));
            else icon = Resources.Load<Sprite>("IngredientsSprites/default");

            Debug.Log("Count: " + slotData.ingredientInStock);
            slot.SetVisual(icon, slotData.ingredientInStock.ToString());
        }

        List<IngredientData> savedSlots = InventoryFactory.LoadGoods();
        foreach (var slotData in savedSlots)
        {
            if (slotData.id == null || slotData.id == "") continue;
            InventorySlotAbstract slot = Instantiate(inventorySlotPrefab, inventoryContentParent).GetComponent<InventorySlotAbstract>();

            Sprite icon;
            if (slotData.id.IndexOf('_') < 0)
            {
                if (Resources.Load<Sprite>("IngredientsSprites/" + slotData.id) != null) icon = Resources.Load<Sprite>("IngredientsSprites/" + slotData.id);
                else icon = Resources.Load<Sprite>("IngredientsSprites/default");
            }
            else if (Resources.Load<Sprite>("IngredientsSprites/" + slotData.id.Substring(0, slotData.id.IndexOf('_'))) != null)
                icon = Resources.Load<Sprite>("IngredientsSprites/" + slotData.id.Substring(0, slotData.id.IndexOf('_')));
            else icon = Resources.Load<Sprite>("IngredientsSprites/default");

            slot.SetVisual(icon, slotData.count.ToString());
        }

        List<EffectData> playerEffects = PlayerConfig.LoadDish();
        foreach (var effectData in playerEffects)
        {
            if (effectData.efcName == null || effectData.efcName == "") continue;
            InventorySlotAbstract slot = Instantiate(inventorySlotPrefab, effectsContentParent).GetComponent<InventorySlotAbstract>();
            Sprite icon;
            if (Resources.Load<Sprite>("EffectsSprites/" + effectData.efcName) != null) icon = Resources.Load<Sprite>("EffectsSprites/" + effectData.efcName);
            else icon = Resources.Load<Sprite>("EffectsSprites/default");
            slot.SetVisual(icon, effectData.screenCount.ToString());
        }
    }

    public void BackToHome()
    {
        SceneManager.LoadScene("LoadingScene");
    }
}
