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
        }
        for (int i = 0; i < 3; i++)
        {
            MixSlot slot = Instantiate(mixSlotPref, rightContent).GetComponent<MixSlot>();
            rightIngredients.Add(slot);
            slot.GetComponent<Button>().onClick.AddListener(slot.Clear);
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

    private void OnResultButtonClick()
    {
        
    }
}
