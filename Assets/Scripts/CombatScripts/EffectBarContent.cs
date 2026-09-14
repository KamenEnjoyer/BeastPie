using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System.Collections.Generic;

public class EffectBarContent : MonoBehaviour
{
    public static EffectBarContent Instance;

    public Transform contentParent;
    public GameObject potionSlotPrefab;

    private void Awake()
    {
        Instance = this;
    }

    public void AddEffect(EffectData effectData)
    {
        EffectSlot slot = Instantiate(potionSlotPrefab, contentParent).GetComponent<EffectSlot>();
        slot.Setup(effectData);
    }
}
