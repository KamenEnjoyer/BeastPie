using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class StorageContent : MonoBehaviour
{
    public static StorageContent Instance;

    public Transform contentParent;     
    public GameObject slotPrefab;

    private List<StorageContentData> ingredientsInStorage = new List<StorageContentData>();
    private List<StorageContentData> allIngredients = new List<StorageContentData>();

    public StorageFilters.StorageFilter currentFilters { get; set; } = StorageFilters.StorageFilter.None;

    private List<StorageSlot> slots = new List<StorageSlot>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateIngredients();
        InventoryContent.Instance.RefreshAllSlots();
    }

    public void UpdateIngredients()
    {
        ingredientsInStorage.Clear();
        allIngredients.Clear();

        List<IngredientData> ingredients = new List<IngredientData>();
            
        ingredients.AddRange(IngredientFactory.LoadIngredients(ScenesConfig.IsHome ? "Home" : "Camp"));

        ingredients.Add(new IngredientData { id = "water", count = -1 });
        ingredients.Add(new IngredientData { id = "fire", count = -1 });
        GILFactory.LoadIngredients();

        foreach (var ingredient in ingredients)
        {
            if (ingredient.count < 1 && ingredient.count > -1) continue;

            StorageContentData newIngredient = new StorageContentData();
            GILData data = GILFactory.FindIngredientById(ingredient.id);
            if (data != null)
            {
                newIngredient.data = data;
                newIngredient.count = ingredient.count;
                if (newIngredient.data.type == GILData.IngredientType.Loot) newIngredient.icon = Resources.Load<Sprite>("IngredientsSprites/" + ingredient.id.Substring(0, ingredient.id.Length - 2));
                else if (Resources.Load<Sprite>("IngredientsSprites/" + ingredient.id) != null) newIngredient.icon = Resources.Load<Sprite>("IngredientsSprites/" + ingredient.id);
                else newIngredient.icon = Resources.Load<Sprite>("IngredientsSprites/default");
                allIngredients.Add(newIngredient);
            }
        }

        StorageFiltersPanel.Instance.ApplyFilter(allIngredients, ingredientsInStorage);
    }

    public void UpdateOneIngredient(string id, int count)
    {
        foreach (var ingInStorage in allIngredients)
        {
            if (ingInStorage.data.id == id)
            {
                ingInStorage.count = count;
                StorageFiltersPanel.Instance.ApplyFilter(allIngredients, ingredientsInStorage);
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
        else if (Resources.Load<Sprite>("IngredientsSprites/" + id) != null) newIngredient.icon = Resources.Load<Sprite>("IngredientsSprites/" + id);
        else newIngredient.icon = Resources.Load<Sprite>("IngredientsSprites/default");
        allIngredients.Add(newIngredient);
        StorageFiltersPanel.Instance.ApplyFilter(allIngredients, ingredientsInStorage);
    }

    public List<StorageContentData> GetAllIngredients()
    {
        return allIngredients;
    }
    public List<StorageContentData> GetFilteredIngredients()
    {
        return ingredientsInStorage;
    }

    public void GenerateInventory()
    {
        foreach (Transform child in contentParent) Destroy(child.gameObject);
        slots.Clear();

        foreach (var ingredient in ingredientsInStorage)
        {
            StorageSlot slot = Instantiate(slotPrefab, contentParent).GetComponent<StorageSlot>();
            slot.Setup(ingredient);
            slots.Add(slot);
        }
    }

    public StorageContentData GetIngredientById(string id)
    {
        StorageContentData data = ingredientsInStorage.FirstOrDefault(i => i.data.id == id);
        if (data != null) return data;
        Debug.LogWarning($"Ingredient with ID {id} not found in storage.");
        return null;
    }
}
