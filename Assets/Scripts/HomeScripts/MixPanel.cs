using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class MixPanel : MonoBehaviour
{
    public static MixPanel Instance;

    public GameObject mixPotionBarPref;
    public GameObject mixDishBarPref;
    public GameObject scrollRectParent;

    public bool isPotionMode = true;

    private bool scrollLock = false;

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

        scrollRectParent.GetComponent<ScrollRect>().decelerationRate = scrollRectParent.GetComponent<RectTransform>().rect.width + 0.2f;
        scrollRectParent.GetComponent<ScrollRect>().onValueChanged.AddListener(OnScrollChanged);
    }

    private void OnScrollChanged(Vector2 position)
    {
        if (scrollLock) return; //Возможна проблема, что при спаме скролла значение мода смешения не будет актуальным.

        if (position.x > 1f)
        {
            StartCoroutine(SetNormalPosotion(1f, 1.2f));
            isPotionMode = false;
            MixPotionMode.Instance.ClearAllIngredients();
        }
        else if (position.x < 0f)
        {
            StartCoroutine(SetNormalPosotion(0f, -0.2f));
            isPotionMode = true;
            MixDishMode.Instance.ClearAllIngredients();
        }
    }

    public IEnumerator SetNormalPosotion(float targetPosition, float startPosition)
    {
        scrollLock = true;

        float duration = 0.3f;
        float elapsed = 0f;
        scrollRectParent.GetComponent<ScrollRect>().horizontalNormalizedPosition = startPosition;


        float trip = targetPosition - startPosition;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        scrollRectParent.GetComponent<ScrollRect>().horizontalNormalizedPosition = targetPosition;
        scrollLock = false;
    }
}
