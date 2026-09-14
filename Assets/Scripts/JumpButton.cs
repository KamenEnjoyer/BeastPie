using UnityEngine;
using System.Collections;

public class JumpButton : MonoBehaviour
{
    public static JumpButton Instance;

    private void Awake()
    {
        Instance = this;
    }

    public IEnumerator Jump(Transform buttonTransform)
    {
        Vector3 originalPos = buttonTransform.localPosition;

        float duration = 0.2f;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            buttonTransform.localPosition = originalPos + new Vector3(0, elapsed * 100, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }
        while (elapsed > 0)
        {
            buttonTransform.localPosition = originalPos + new Vector3(0, elapsed * 100, 0);

            elapsed -= Time.deltaTime;
            yield return null;
        }

        buttonTransform.localPosition = originalPos;
    }
}
