using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Theme : MonoBehaviour
{
    public static Theme Instance;

    public List<Image> yellowOrRedColour;
    public List<GameObject> activeOrNotObjects;

    private void Awake()
    {
        Instance = this;
        foreach (Image image in yellowOrRedColour)
        {
            image.color = ScenesConfig.IsHome ? new Color(1f, 0.9f, 0.3f) : new Color(0.9f, 0.6f, 0.4f);
        }
        foreach (GameObject obj in activeOrNotObjects)
        {
            obj.SetActive(ScenesConfig.IsHome);
        }
    }
}
