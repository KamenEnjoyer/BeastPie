using UnityEngine;

public class ProjectileSpawn : MonoBehaviour
{
    public float damage = 5f;

    private float tension;
    private float speed = 10f;
    private Vector2 direction;
    private Camera mainCamera;
    private Vector2 screenMin;
    private Vector2 screenMax;

    private bool isEnemy = false;

    private Rigidbody2D rb;

    private void FixedUpdate()
    {
        rb.MovePosition(
            rb.position + direction * speed * Time.fixedDeltaTime
        );

        if (transform.position.x < screenMin.x || transform.position.x > screenMax.x ||
            transform.position.y < screenMin.y || transform.position.y > screenMax.y)
        {
            Destroy(gameObject);
        }
    }

    public void Init(Vector2 dir, float tension, bool isEnemy, float speed)
    {
        this.speed = speed;
        this.isEnemy = isEnemy;

        direction = dir.normalized;
        this.tension = tension;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);

        mainCamera = Camera.main;
        screenMin = mainCamera.ViewportToWorldPoint(new Vector2(0, 0));
        screenMax = mainCamera.ViewportToWorldPoint(new Vector2(1, 1));

        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            Destroy(gameObject);
            return;
        }

        if (other.CompareTag("Player") && isEnemy)
        {
            PlayerHealth.Instance.TakeDamage(damage);
        }
        else if (other.CompareTag("Enemy"))
        {
            if (isEnemy) other.GetComponent<Enemy>()?.TakeDamage(damage / 2f);
            else other.GetComponent<Enemy>()?.TakeDamage(damage * tension);
        }
        Destroy(gameObject);
    }

}
