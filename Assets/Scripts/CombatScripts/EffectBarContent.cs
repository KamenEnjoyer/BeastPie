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

    private void Start()
    {
        if (PlayerConfig.LoadDish() == null) return;
        else
        {
            List<EffectData> effects = new List<EffectData>();
            foreach (var eff in PlayerConfig.LoadDish())
            {
                PotionEffect effect = EffectRegistry.GetPotionEffect(eff.efcName);
                if (effect == null) Debug.LogError("Effect " + eff + " in PotionBarSlot not found.");
                else
                {
                    effect.ApplyDish();
                    EffectSlot slot = Instantiate(potionSlotPrefab, contentParent).GetComponent<EffectSlot>();
                    slot.Setup(eff, true);

                    eff.screenCount--;
                    if (eff.screenCount > 0) effects.Add(eff);
                }
            }
            PlayerConfig.SaveResources(effects);
        }
    }

    public void AddEffect(EffectData effectData)
    {
        EffectSlot slot = Instantiate(potionSlotPrefab, contentParent).GetComponent<EffectSlot>();
        slot.Setup(effectData);
    }
}
