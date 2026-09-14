using UnityEngine;

public class Spitter : EnemyBehaviour
{
    public GameObject projectilePrefab;
    public float shootInterval = 2f;
    public float moveSpeed = 1f;
    public float damage = 1f;

    private float timer;

    public override void OnUpdate()
    {
        if (!player) return;

        timer -= Time.deltaTime;
        if (timer > 0f) return;

        Shoot();
        timer = shootInterval;
    }

    private void Shoot()
    {
        Vector2 shootDirection = transform.up;
        Vector2 spawnPosition = (Vector2)transform.position + shootDirection * 0.6f;

        GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);
        projectile.GetComponent<ProjectileSpawn>().Init(shootDirection, damage, true, 7f);
    }

    public override void OnFixedUpdate()
    {
        if (!player) return;

        Vector2 dir = ((Vector2)player.position - rb.position).normalized;
        float distance = Mathf.Sqrt(
            (player.position.x - rb.position.x) *
            (player.position.x - rb.position.x) +
            (player.position.y - rb.position.y) *
            (player.position.y - rb.position.y)
        );
        if (distance < 10f)
        {
            return;
        }
        rb.MovePosition(rb.position + dir * moveSpeed * Time.fixedDeltaTime);
    }
}
