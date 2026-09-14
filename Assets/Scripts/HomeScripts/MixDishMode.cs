using UnityEngine;

public class MixDishMode : MonoBehaviour
{
    public static MixDishMode Instance;

    public Transform leftContent;
    public Transform rightContent;
    public Transform middleContent;
    public GameObject mixSlotPref;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {/*
        leftIngredientButton = Instantiate(mixSlotPref, ingButtonsContent);
        leftIngredient = leftIngredientButton.GetComponent<MixSlot>();
        leftIngredientButton.GetComponent<Button>().onClick.AddListener(() => { leftIngredient.Clear(); });

        rightIngredientButton = Instantiate(mixSlotPref, ingButtonsContent);
        rightIngredient = rightIngredientButton.GetComponent<MixSlot>();
        rightIngredientButton.GetComponent<Button>().onClick.AddListener(() => { rightIngredient.Clear(); });

        resultButton = Instantiate(mixSlotPref, resultButtonContent);
        resultButton.GetComponent<Button>().onClick.AddListener(OnResultButtonClick);
        result = resultButton.GetComponent<MixSlot>();
        result.SetSlotScale();

        ClearAllIngredients();*/
    }
}
