using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class PotionBarSlot : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI countText;
    public TextMeshProUGUI keyText;
    public Image cooldown;

    private GILData ingredientData;
    private int count;
    private int countAtStart;

    private float cooldownDuration = 2.5f;
    private bool onCooldown = false;

    public void Setup(InventorySaveData data)
    {
        string iconPath = "IngredientsSprites/" + data.ingredientId.Substring(0, data.ingredientId.Length - 2);
        if (Resources.Load<Sprite>(iconPath) != null) icon.sprite = Resources.Load<Sprite>(iconPath);
        else icon.sprite = Resources.Load<Sprite>("IngredientsSprites/default");
        count = data.ingredientInStock;
        countAtStart = count;
        keyText.text = data.key.Replace("Alpha", "");
        ingredientData = GILFactory.FindIngredientById(data.ingredientId);

        UpdateView();
    }

    public void ConsumeOne()
    {
        if (count <= 0 || onCooldown)
        {
            ShakeButton.Instance.Shake(icon.transform.parent, "", true);
            return;
        }
        foreach (var effectId in ingredientData.effectIds)
        {
            PotionEffect effect = EffectRegistry.GetPotionEffect(effectId);
            if (effect == null) Debug.LogError("Effect " + effectId + " in PotionBarSlot not found.");
            else
            {
                effect.Setup(ingredientData.density, ingredientData.price);
                effect.ApplyPotion();
                EffectBarContent.Instance.AddEffect(effect.GetEffectData());
            }
        }

        JumpButton.Instance.StartCoroutine(JumpButton.Instance.Jump(icon.transform.parent));
        count--;

        StartCoroutine(CooldownCoroutine());
        UpdateView();
    }

    private IEnumerator CooldownCoroutine() 
    { 
        onCooldown = true; 
        cooldown.fillAmount = 1f; 
        float timer = 0f; 
        while (timer < cooldownDuration) 
        { 
            timer += Time.deltaTime; 
            cooldown.fillAmount = 1f - (timer / cooldownDuration); 
            yield return null; 
        } 
        cooldown.fillAmount = 0f; 
        onCooldown = false; 
    }

    private void UpdateView()
    {
        countText.text = count.ToString();

        if (count == 0)
        {
            icon.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
        }
        else
        {
            icon.color = new Color(1f, 1f, 1f, 1f);
        }
    }
    
    public string GetIngredientId()
    {
        return ingredientData.id;
    }

    public void SaveNewPotionCount()
    {
        if (!IngredientFactory.RemoveIngredient(ingredientData.id, countAtStart - count, true))
        {
            IngredientFactory.AddIngredient(ingredientData.id, count, "camp");
        }
        InventoryFactory.SaveCountsByIngredient(ingredientData.id, IngredientFactory.GetCountById(ingredientData.id));
    }
}
