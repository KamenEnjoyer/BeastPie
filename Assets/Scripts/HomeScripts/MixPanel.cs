using System;
using UnityEngine;
using UnityEngine.UI;

public class MixPanel : MonoBehaviour
{
    public static MixPanel Instance;

    public GameObject mixPotionBarPref;
    public GameObject mixDishBarPref;
    public GameObject scrollRectParent;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Canvas.ForceUpdateCanvases();

        float panelHeight = scrollRectParent.GetComponent<RectTransform>().rect.height;
        Instantiate(mixPotionBarPref, gameObject.GetComponent<Transform>()).GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, panelHeight);
        Instantiate(mixDishBarPref, gameObject.GetComponent<Transform>()).GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, panelHeight);
    }
}
