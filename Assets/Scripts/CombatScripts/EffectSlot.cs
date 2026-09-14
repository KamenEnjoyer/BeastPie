using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EffectSlot : MonoBehaviour
{
    public Image icon;
    public Image cooldown;

    bool onCooldown = true;

    public void Setup(EffectData effect)
    {
        icon.sprite = Resources.Load<Sprite>("EffectsSprites/" + effect.efcName);

        StartCoroutine(CooldownCoroutine(effect.cooldownDuration));
    }

    private IEnumerator CooldownCoroutine(float cooldownDuration)
    {
        float timer = 0f;
        while (timer < cooldownDuration)
        {
            timer += Time.deltaTime;
            cooldown.fillAmount = 1f - (timer / cooldownDuration);
            yield return null;
        }
        onCooldown = false;
    }

    private void Update()
    {
        if (!onCooldown)
        {
            Destroy(gameObject);
        }
    }
}