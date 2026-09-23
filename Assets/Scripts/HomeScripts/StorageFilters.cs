using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class StorageFilters : MonoBehaviour
{
    public static StorageFilters Instance;

    public Button sortByNameButton;
    public Button sortByDensityButton;
    public Button sortByQuantityButton;
    public Button sortByDefaultButton;
    public Toggle catalystToggle;
    public Toggle potionToggle;
    public Toggle lootToggle;
    public Toggle foodToggle;
    public Toggle otherToggle;
    public Toggle inStockToggle;

    [System.Flags]
    public enum StorageFilter
    {
        None = 0,
        Catalyst = 1 << 0,
        Potion = 1 << 1,
        Loot = 1 << 2,
        Food = 1 << 3,
        Other = 1 << 4,
        InStock = 1 << 5
    }
    public StorageFilter activeFilter = StorageFilter.None;

    public void Awake()
    {
        Instance = this;

        StorageFiltersPanel.Instance.SetFilterButtons(sortByNameButton, sortByDensityButton, sortByQuantityButton, sortByDefaultButton);

        catalystToggle.onValueChanged.AddListener(value => SetFilter(StorageFilter.Catalyst, value));
        potionToggle.onValueChanged.AddListener(value => SetFilter(StorageFilter.Potion, value));
        lootToggle.onValueChanged.AddListener(value => SetFilter(StorageFilter.Loot, value));
        foodToggle.onValueChanged.AddListener(value => SetFilter(StorageFilter.Food, value));
        otherToggle.onValueChanged.AddListener(value => SetFilter(StorageFilter.Other, value));
        inStockToggle.onValueChanged.AddListener(value => SetFilter(StorageFilter.InStock, value));
    }

    private void SetFilter(StorageFilter filter, bool enabled)
    {
        if (enabled) activeFilter |= filter;
        else activeFilter &= ~filter;

        StorageFiltersPanel.Instance.SetActiveFilter(activeFilter);
    }
}

