using UnityEngine;
using System.Collections;

public class ShakeButton : MonoBehaviour
{
    Coroutine currentCoroutine;

    public static ShakeButton Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void Shake(Transform buttonTransform, string message, bool allowBug = true)
    {
        if (message != "") Description.Instance.ShowMessage(message, 2f);
        if (allowBug)
        {
            StartCoroutine(RunShaking(buttonTransform));
        }
        else if (currentCoroutine == null) currentCoroutine = StartCoroutine(RunShaking(buttonTransform));
    }

    public IEnumerator RunShaking(Transform buttonTransform)
    {
        Vector3 originalPos = buttonTransform.localPosition;

        float duration = 0.3f;
        float magnitude = 10f;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            float offset = Mathf.Sin(elapsed * 50f) * magnitude;
            buttonTransform.localPosition = originalPos + new Vector3(offset, 0, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        buttonTransform.localPosition = originalPos;
        currentCoroutine = null;
    }
}
