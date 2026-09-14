using UnityEngine;

public class Buba : EnemyBehaviour
{
    public float moveSpeed = 2.5f;
    public float damage = 1f;
    public float damageInterval = 1f;
    private float damageTimer = 0f;

    public override void OnUpdate()
    {
        Vector2 direction = player.position - transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    public override void OnFixedUpdate()
    {
        if (!player) return;

        Vector2 dir = ((Vector2)player.position - rb.position).normalized;
        rb.MovePosition(rb.position + dir * moveSpeed * Time.fixedDeltaTime);

        if (damageTimer > 0f) damageTimer -= Time.fixedDeltaTime;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!(other.gameObject.layer == LayerMask.NameToLayer("PlayerTrigger"))) return;

        if (damageTimer <= 0f)
        {
            PlayerHealth.Instance.TakeDamage(damage);
            damageTimer = damageInterval;
        }
    }
}
