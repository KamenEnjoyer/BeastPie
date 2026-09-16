using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MixDishMode : MonoBehaviour
{
    public static MixDishMode Instance;

    public Transform leftContent;
    public Transform rightContent;
    public Transform resultContent;
    public Transform catalystContent;
    public GameObject mixSlotPref;

    private List<GameObject> leftIngredientsButton;
    private List<GameObject> rightIngredientsButton;
    private GameObject catalystButton;
    private GameObject resultButton;

    private List<MixSlot> leftIngredients = new List<MixSlot>();
    private List<MixSlot> rightIngredients = new List<MixSlot>();
    private MixSlot catalyst;
    private MixSlot result;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        for(int i = 0; i < 3; i++)
        {
            MixSlot slot = Instantiate(mixSlotPref, leftContent).GetComponent<MixSlot>();
            leftIngredients.Add(slot);
            slot.GetComponent<Button>().onClick.AddListener(slot.Clear);
            slot.SetSlotScale(0.8f);
        }
        for (int i = 0; i < 3; i++)
        {
            MixSlot slot = Instantiate(mixSlotPref, rightContent).GetComponent<MixSlot>();
            rightIngredients.Add(slot);
            slot.GetComponent<Button>().onClick.AddListener(slot.Clear);
            slot.SetSlotScale(0.8f);
        }

        resultButton = Instantiate(mixSlotPref, resultContent);
        resultButton.GetComponent<Button>().onClick.AddListener(OnResultButtonClick);
        result = resultButton.GetComponent<MixSlot>();
        result.SetSlotScale();

        catalystButton = Instantiate(mixSlotPref, catalystContent);
        catalyst = catalystButton.GetComponent<MixSlot>();
        catalystButton.GetComponent<Button>().onClick.AddListener(() => { catalyst.Clear(); });
        catalyst.SetSlotScale(0.9f);

        ClearAllIngredients();
    }

    public void AddLeftIngredient(StorageContentData ingredient)
    {
        if (ingredient.data.type != GILData.IngredientType.Loot)
        {
            ShakeButton.Instance.Shake(leftContent, "The base of the dish requires an ingredient from a monster.");
            return;
        }
        foreach (var slot in leftIngredients)
        {
            if (slot.GetIng() == null)
            {
                slot.Setup(ingredient);
                UpdateResult();
                return;
            }
            if (slot.GetIng().data.id == ingredient.data.id)
            {
                ShakeButton.Instance.Shake(leftContent, "You cannot add the same ingredient twice.");
                return;
            }
        }
        ShakeButton.Instance.Shake(leftContent, "You cannot add more than 3 ingredients.");
    }

    public void AddRightIngredient(StorageContentData ingredient)
    {
        if (ingredient.data.type != GILData.IngredientType.Food)
        {
            ShakeButton.Instance.Shake(rightContent, "The base of the dish requires a food ingredient.");
            return;
        }
        foreach (var slot in rightIngredients)
        {
            if (slot.GetIng() == null)
            {
                slot.Setup(ingredient);
                UpdateResult();
                return;
            }
            if (slot.GetIng().data.id == ingredient.data.id)
            {
                ShakeButton.Instance.Shake(rightContent, "You cannot add the same ingredient twice.");
                return;
            }
        }
        ShakeButton.Instance.Shake(rightContent, "You cannot add more than 3 ingredients.");
    }

    public void SetCatalystIngredient(StorageContentData ingredient)
    {
        if (ingredient.data.type != GILData.IngredientType.Catalyst)
        {
            ShakeButton.Instance.Shake(catalystContent, "You can only add a catalyst ingredient.");
            return;
        }
        catalyst.Setup(ingredient);
        UpdateResult();
    }

    private void UpdateResult()
    {
        if (!IngredientsExist(rightIngredients) || !IngredientsExist(leftIngredients))
        {
            result.Clear();
            return;
        }
        if (catalyst.GetIng() == null)
        {
            result.Clear();
            return;
        }

        StorageContentData finalIngredient = Mix(leftIngredients, rightIngredients, catalyst.GetIng());

        if (finalIngredient != null) result.Setup(finalIngredient);
        else result.Clear();
    }

    private bool IngredientsExist(List<MixSlot> slots)
    {
        foreach (var slot in slots)
        {
            if (slot.GetIng() != null)
            {
                return true;
            }
        }
        return false;
    }

    private void ClearAllIngredients()
    {
        foreach (var slot in leftIngredients)
        {
            slot.Clear();
        }
        foreach (var slot in rightIngredients)
        {
            slot.Clear();
        }
        catalyst.Clear();
        result.Clear();
    }

    private StorageContentData Mix(List<MixSlot> leftSlots, List<MixSlot> rightSlots, StorageContentData catalyst)
    {

        return null;
    }

    private void OnResultButtonClick()
    {
        
    }
}
