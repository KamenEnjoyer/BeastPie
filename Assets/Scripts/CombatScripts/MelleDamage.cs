using UnityEngine;

public class MelleDamage : MonoBehaviour
{
    public float damage = 1f;
    public bool isPlayer = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") && isPlayer)
        {
            other.GetComponent<Enemy>()?.TakeDamage(damage);
        }
        else if (other.CompareTag("Player") && !isPlayer)
        {
            PlayerHealth.Instance.TakeDamage(damage);
        }
    }
}
