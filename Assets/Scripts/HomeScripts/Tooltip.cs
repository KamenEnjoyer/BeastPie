using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class TooltipUI : MonoBehaviour
{
    public static TooltipUI Instance;

    [SerializeField] private GameObject tipsPanel;
    [SerializeField] private TMP_Text tooltipText;
    [SerializeField] private RectTransform tipsRect;

    private CanvasGroup canvasGroup;
    private Coroutine showCoroutine;
    private Coroutine fadeCoroutine;

    private void Awake()
    {
        Instance = this;

        canvasGroup = tipsPanel.GetComponent<CanvasGroup>();

        if (canvasGroup == null)
            canvasGroup = tipsPanel.AddComponent<CanvasGroup>();

        tipsPanel.SetActive(false);
        canvasGroup.alpha = 0f;
    }

    public void ShowDelayed(string message, Vector3 position, float delay)
    {
        if (showCoroutine != null)
            StopCoroutine(showCoroutine);

        showCoroutine = StartCoroutine(ShowWithDelay(message, position, delay));
    }

    private IEnumerator ShowWithDelay(string msg, Vector3 pos, float delay)
    {
        yield return new WaitForSeconds(delay);

        tooltipText.text = msg;
        LayoutRebuilder.ForceRebuildLayoutImmediate(tipsRect);

        tipsPanel.SetActive(true);
        tipsPanel.transform.position = pos;

        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine); //REFACTOR

        fadeCoroutine = StartCoroutine(FadeCanvasGroup(0f, 1f, 0.3f));
    }

    private IEnumerator FadeCanvasGroup(float from, float to, float duration)
    {
        float elapsed = 0f;
        canvasGroup.alpha = from;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(from, to, elapsed / duration);
            yield return null;
        }
        canvasGroup.alpha = to;
    }

    public void HideImmediate()
    {
        if (showCoroutine != null)
            StopCoroutine(showCoroutine);

        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        canvasGroup.alpha = 0f;
        tipsPanel.SetActive(false);
    }
}
