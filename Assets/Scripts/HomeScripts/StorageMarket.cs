using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class StorageMarket : MonoBehaviour
{
    public static StorageMarket Instance;

    public GameObject leftPanel;
    public GameObject rightPanel;
    public GameObject tradeIcon;
    public GameObject price;
    public GameObject amount;
    public GameObject description;

    public TMP_Text coinsText;
    public Transform contentParent;
    public GameObject slotPrefab;

    public Button buyButton;
    public Button sellButton;

    private int coinsCount;
    private int ingredientToBuyCount;
    private StorageContentData ingredientToBuy;

    private List<StorageContentData> goodsInMarket = new List<StorageContentData>();
    private List<StorageContentData> allGoods = new List<StorageContentData>();

    public StorageFilters.StorageFilter currentFilters { get; set; } = StorageFilters.StorageFilter.None;

    private List<StorageSlot> slots = new List<StorageSlot>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        coinsCount = ResourcesFactory.GetCoinsQuantity(true);
        coinsText.text = coinsCount.ToString();
        SetPanelTransform();
        UpdateIngredients();
        Description.Instance.OpenMarket(description.GetComponent<TMP_Text>());
    }

    public void SetPanelTransform()
    {
        float panelWidth = GetComponent<RectTransform>().rect.width;

        leftPanel.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, panelWidth * 0.53f);
        rightPanel.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, panelWidth * 0.47f);

        float panelHeight = GetComponent<RectTransform>().rect.height - 45f;

        tradeIcon.transform.parent.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, panelHeight * 0.4f);
        price.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, panelHeight * 0.1f);
        amount.GetComponentInParent<Image>().GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, panelHeight * 0.1f);
        description.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, panelHeight * 0.4f);
    }

    public void UpdateIngredients()
    {
        goodsInMarket.Clear();
        allGoods.Clear();

        List<FoodIngredientData> defaultData = new List<FoodIngredientData>(Resources.LoadAll<FoodIngredientData>("IngredientsTypes/Food/"));

        foreach (var ingredient in defaultData)
        {
            StorageContentData newIngredient = new StorageContentData();
            newIngredient.data = new GILData();
            newIngredient.data.id = ingredient.id;
            newIngredient.data.type = GILData.IngredientType.Food;
            newIngredient.data.ingName = LocalizationSettings.StringDatabase.GetLocalizedString("IngredientsNamesLocalization", ingredient.id);
            newIngredient.data.description = LocalizationSettings.StringDatabase.GetLocalizedString("IngredientsDescriptionsLocalization", ingredient.id);
            newIngredient.data.effect = LocalizationSettings.StringDatabase.GetLocalizedString("IngredientsEffectsLocalization", ingredient.id);

            newIngredient.data.dishEffectIds = new List<string>(ingredient.effectIds);
            newIngredient.data.conflictIds = new List<string>(ingredient.conflictIds);
            newIngredient.data.price = ingredient.price;

            newIngredient.count = Random.Range(ingredient.minQuantity, ingredient.maxQuantity);

            if (Resources.Load<Sprite>("IngredientsSprites/" + ingredient.id) != null) newIngredient.icon = Resources.Load<Sprite>("IngredientsSprites/" + ingredient.id);
            else newIngredient.icon = Resources.Load<Sprite>("IngredientsSprites/default");

            allGoods.Add(newIngredient);
        }
        StorageFiltersPanel.Instance.ApplyFilter(allGoods, goodsInMarket);
    }

    public void UpdateOneIngredient(string id, int count)
    {
        foreach (var ingInStorage in allGoods)
        {
            if (ingInStorage.data.id == id)
            {
                ingInStorage.count += count;
                StorageFiltersPanel.Instance.ApplyFilter(allGoods, goodsInMarket);
                return;
            }
        }
        StorageContentData newIngredient = new StorageContentData
        {
            data = GILFactory.FindIngredientById(id),
            count = count
        };
        Debug.Log($"Adding new ingredient to storage: {newIngredient.data.ingName} with count {count} and type {newIngredient.data.type}");
        if (newIngredient.data.type == GILData.IngredientType.Loot) newIngredient.icon = Resources.Load<Sprite>("IngredientsSprites/" + id.Substring(0, id.Length - 2));
        else newIngredient.icon = Resources.Load<Sprite>("IngredientsSprites/default");
        allGoods.Add(newIngredient);
        StorageFiltersPanel.Instance.ApplyFilter(allGoods, goodsInMarket);
    }

    public void GenerateInventory()
    {
        foreach (Transform child in contentParent) Destroy(child.gameObject);
        slots.Clear();

        foreach (var ingredient in goodsInMarket) 
        {
            StorageSlot slot = Instantiate(slotPrefab, contentParent).GetComponent<StorageSlot>();
            slot.Setup(ingredient);
            slots.Add(slot);
        }
    }

    public void SetIngredientToTrade(StorageContentData ingredient, bool toBuy)
    {
        if (ingredient.count < 1)
        {
            Description.Instance.ShowMessage("This ingredient is out of stock!");
            return;
        }

        if (toBuy)
        {
            buyButton.interactable = true;
            if (ingredient != ingredientToBuy) sellButton.interactable = false;
        }
        else
        {
            if (ingredient != ingredientToBuy) buyButton.interactable = false;
            sellButton.interactable = true;
        }

        tradeIcon.GetComponent<Image>().color = Color.white;
        tradeIcon.GetComponent<Image>().sprite = ingredient.icon;

        price.GetComponent<TMP_Text>().text = "Price: " + (ingredient.data.price * (ingredient.data.density+1) * ingredient.count).ToString();

        ingredientToBuy = ingredient;
        ingredientToBuyCount = ingredient.count;

        amount.GetComponent<TMP_Text>().text = ingredientToBuyCount.ToString();
    }

    public void ChangeCount(int count)
    {
        ingredientToBuyCount += count;
        if (ingredientToBuyCount < 1 || ingredientToBuyCount > ingredientToBuy.count)
        {
            ingredientToBuyCount -= count;
            return;
        }
        amount.GetComponent<TMP_Text>().text = ingredientToBuyCount.ToString();
        price.GetComponent<TMP_Text>().text = "Price: " + (ingredientToBuy.data.price * (ingredientToBuy.data.density + 1) * ingredientToBuyCount).ToString();
    }

    public void ClearCount()
    {
        buyButton.interactable = false;
        sellButton.interactable = false;
        tradeIcon.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
        amount.GetComponent<TMP_Text>().text = "0";
        price.GetComponent<TMP_Text>().text = "";
    }

    public void BuyIngredient()
    { 
        int totalPrice = ingredientToBuy.data.price * (ingredientToBuy.data.density + 1) * ingredientToBuyCount;
        if (ResourcesFactory.GetCoinsQuantity() < totalPrice)
        {
            Description.Instance.ShowMessage("Not enough coins to buy this!");
            return;
        }

        StorageMarketPanel.Instance.UpdateCoins(-totalPrice);
        coinsCount += totalPrice;
        coinsText.text = coinsCount.ToString();
        ResourcesFactory.AddCoins(totalPrice, true);

        ingredientToBuy.count -= ingredientToBuyCount;
        ClearCount();

        if (GILFactory.FindIngredientById(ingredientToBuy.data.id) == null)
        {
            GILFactory.AddIngredientFromDefaulds(ingredientToBuy.data.id, ingredientToBuy.data.type);
        }

        UpdateOneIngredient(ingredientToBuy.data.id, ingredientToBuy.count);

        IngredientFactory.AddIngredient(ingredientToBuy.data.id, ingredientToBuyCount, "Home");
    }

    public void SellIngredient()
    {
        int totalPrice = ingredientToBuy.data.price * (ingredientToBuy.data.density + 1) * ingredientToBuyCount;
        if (ResourcesFactory.GetCoinsQuantity(true) < totalPrice)
        {
            Description.Instance.ShowMessage("Trader has not enough coins to sell this!");
            return;
        }

        StorageMarketPanel.Instance.UpdateCoins(totalPrice);
        coinsCount -= totalPrice;
        coinsText.text = coinsCount.ToString();
        ResourcesFactory.AddCoins(-totalPrice, true);

        ingredientToBuy.count -= ingredientToBuyCount;
        ClearCount();

        UpdateOneIngredient(ingredientToBuy.data.id, ingredientToBuyCount);

        IngredientFactory.AddIngredient(ingredientToBuy.data.id, -ingredientToBuyCount, "Home");
    }

    public List<StorageContentData> GetAllGoods()
    {
        return allGoods;
    }
    public List<StorageContentData> GetFilteredGoods()
    {
        return goodsInMarket;
    }
    public StorageContentData GetIngredientById(string id)
    {
        StorageContentData data = allGoods.FirstOrDefault(i => i.data.id == id);
        if (data != null) return data;
        Debug.LogWarning($"Ingredient with ID {id} not found in Market.");
        return null;
    }
}
