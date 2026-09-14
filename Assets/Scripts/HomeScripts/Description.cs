using UnityEngine;
using TMPro;
using System.Collections;

public class Description : MonoBehaviour
{
    public static Description Instance;

    [SerializeField] private TMP_Text descriptionText;
    private TMP_Text mainDescriptionText;

    private Coroutine clearCoroutine;

    private void Awake()
    {
        Instance = this;
    }

    public void ShowDescription(string text)
    {
        if (clearCoroutine != null)
        {
            StopCoroutine(clearCoroutine);
            clearCoroutine = null;
        }
        descriptionText.text = text;
    }

    public void ClearDescription()
    {
        if (clearCoroutine != null)
        {
            StopCoroutine(clearCoroutine);
            clearCoroutine = null;
        }
        descriptionText.text = "";
    }

    public void ShowMessage(string text, float duration = 2f)
    {
        if (clearCoroutine != null) StopCoroutine(clearCoroutine);
        descriptionText.text = text;
        clearCoroutine = StartCoroutine(ClearAfterDelay(duration));
    }

    private IEnumerator ClearAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        descriptionText.text = "";
        clearCoroutine = null;
    }

    public void OpenMarket(TMP_Text text)
    {
        mainDescriptionText = descriptionText;
        descriptionText = text;
    }

    public void CloseMarket()
    {
        descriptionText = mainDescriptionText;
    }
}
