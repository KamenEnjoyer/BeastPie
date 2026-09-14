using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    public static PlayerHealth Instance;

    private void Awake()
    {
        Instance = this;
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        gameObject.GetComponent<Image>().fillAmount = currentHealth / maxHealth;
        if (currentHealth <= 0f)
        {
            MenuSpawner.Instance.ShowDeathMenu();
        }
    }

    public IEnumerator GradualHeal(float health, float step)
    {
        while (health > 0f)
        {
            if (currentHealth < maxHealth) currentHealth += step;
            gameObject.GetComponent<Image>().fillAmount = currentHealth / maxHealth;
            health -= step;
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void PermanentHeal(float health)
    {
        currentHealth += health;
        gameObject.GetComponent<Image>().fillAmount = currentHealth / maxHealth;
    }
}
