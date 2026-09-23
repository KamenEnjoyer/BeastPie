using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static StorageFilters;
using UnityEngine.UI;

public class StorageFiltersPanel : MonoBehaviour
{
    public static StorageFiltersPanel Instance;

    public Transform parent;
    public GameObject filterPanelPref;
    private GameObject filterPanel;

    private StorageFilter activeFilter = StorageFilter.None;

    private List<StorageContentData> filteredIngredients = new List<StorageContentData>();

    void Awake()
    {
        Instance = this;
    }

    public void SetFilterButtons(Button name, Button density, Button quantity, Button defaultButton)
    {
        name.onClick.AddListener(() => SortingButtonClick(ScenesConfig.sortingVariables.Name));
        density.onClick.AddListener(() => SortingButtonClick(ScenesConfig.sortingVariables.Density));
        quantity.onClick.AddListener(() => SortingButtonClick(ScenesConfig.sortingVariables.Quantity));
        defaultButton.onClick.AddListener(() => SortingButtonClick(ScenesConfig.sortingVariables.Default));
    }
    private void SortingButtonClick(ScenesConfig.sortingVariables sortConfig)
    {
        ScenesConfig.currentSorting = sortConfig;
        SortInventory(StorageContent.Instance.GetFilteredIngredients());
        if (StorageMarketPanel.Instance.IsMarketPanelActive()) SortInventory(StorageMarket.Instance.GetFilteredGoods());
    }

    public void SortInventory(List<StorageContentData> filteredIng)
    {
        filteredIngredients.AddRange(filteredIng);
        filteredIng.Clear();
        switch (ScenesConfig.currentSorting)
        {
            case ScenesConfig.sortingVariables.Name:
                SortByName(filteredIng);
                break;
            case ScenesConfig.sortingVariables.Density:
                SortByDensity(filteredIng);
                break;
            case ScenesConfig.sortingVariables.Quantity:
                SortByQuantity(filteredIng);
                break;
            default:
                SortByDefault(filteredIng);
                break;
        }
        StorageContent.Instance.GenerateInventory();
        if (StorageMarketPanel.Instance.IsMarketPanelActive()) StorageMarket.Instance.GenerateInventory();
        filteredIngredients.Clear();
    }

    void SortByName(List<StorageContentData> filteredIng)
    {
        filteredIng.AddRange(filteredIngredients.OrderBy(i => i.data.ingName).ToList());
    }

    void SortByDensity(List<StorageContentData> filteredIng)
    {
        filteredIng.AddRange(filteredIngredients.OrderByDescending(i => i.data.density).ToList());
    }

    void SortByQuantity(List<StorageContentData> filteredIng)
    {
        filteredIng.AddRange(filteredIngredients.OrderByDescending(i => i.count).ToList());
    }

    void SortByDefault(List<StorageContentData> filteredIng)
    {
        filteredIng.AddRange(filteredIngredients.OrderBy(i => i.data.type).ToList());
    }

    public void ApplyFilter(List<StorageContentData> allIng, List<StorageContentData> filteredIng)
    {
        filteredIng.Clear();
        if (activeFilter == StorageFilter.None)
        {
            filteredIng.AddRange(allIng);
        }
        else
        {
            filteredIng.AddRange(allIng.Where(item =>
            {
                return
                    (activeFilter.HasFlag(StorageFilter.Catalyst) && item.data.type == GILData.IngredientType.Catalyst)
                    ||
                    (activeFilter.HasFlag(StorageFilter.Potion) && item.data.type == GILData.IngredientType.Potion)
                    ||
                    (activeFilter.HasFlag(StorageFilter.Loot) && item.data.type == GILData.IngredientType.Loot)
                    ||
                    (activeFilter.HasFlag(StorageFilter.Food) && item.data.type == GILData.IngredientType.Food)
                    ||
                    (activeFilter.HasFlag(StorageFilter.Other) && item.data.type == GILData.IngredientType.Other)
                    ||
                    (activeFilter.HasFlag(StorageFilter.InStock) && (item.count > 0 || item.count == -1));
            }));
        }
        SortInventory(filteredIng);
    }

    public void SetActiveFilter(StorageFilter filter)
    {
        activeFilter = filter;
        ApplyFilter(StorageContent.Instance.GetAllIngredients(), StorageContent.Instance.GetFilteredIngredients());
        if (StorageMarketPanel.Instance.IsMarketPanelActive()) 
        { 
            ApplyFilter(StorageMarket.Instance.GetAllGoods(), StorageMarket.Instance.GetFilteredGoods());
        }
    }

    public void ShowFilters()
    {
        if (filterPanel != null) Destroy(filterPanel);
        else filterPanel = Instantiate(filterPanelPref, parent, false);
    }

    public void HideFilters()
    {
        if (filterPanel != null) Destroy(filterPanel);
    }
}
