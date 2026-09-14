using UnityEngine;

public class MixPanel : MonoBehaviour
{
    public static MixPanel Instance;

    public GameObject mixPotionBarPref;
    public GameObject mixDishBarPref;
    public Transform contentParent;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Instantiate(mixPotionBarPref, contentParent);
        Instantiate(mixDishBarPref, contentParent);
    }
}
