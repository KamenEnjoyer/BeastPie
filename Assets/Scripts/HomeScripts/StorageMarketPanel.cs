using UnityEngine;
using TMPro;

public class StorageMarketPanel : MonoBehaviour
{
    public static StorageMarketPanel Instance;

    public TMP_Text coinsText;
    public Transform parent;
    public GameObject marketPanelPref;
    private GameObject marketPanel;

    private int coinsCount;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ResourcesFactory.LoadCoins();
        coinsCount = ResourcesFactory.GetCoinsQuantity();
        coinsText.text = coinsCount.ToString();
    }

    public void UpdateCoins(int quantity)
    {
        coinsCount += quantity;
        coinsText.text = coinsCount.ToString();
        ResourcesFactory.AddCoins(coinsCount);
    }

    public void ShowMarket()
    {
        if (marketPanel != null)
        {
            Destroy(marketPanel);
            Description.Instance.CloseMarket();
        }
        else marketPanel = Instantiate(marketPanelPref, parent, false);
        StorageFiltersPanel.Instance.HideFilters();
    }

    public bool IsMarketPanelActive()
    {
        return marketPanel != null;
    }
}
