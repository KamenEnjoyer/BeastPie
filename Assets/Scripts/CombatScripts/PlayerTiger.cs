using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerTiger : MonoBehaviour
{
    protected Rigidbody2D rb;
    public float moveSpeed = 10f;

    private Vector2 startPos;
    private Vector2 endPos;
    private GameObject enemy;
    private bool isReturning = false;

    public static PlayerTiger Instance;

    public void Initialize(GameObject enemy)
    {
        this.enemy = enemy;

        startPos = transform.position;
        endPos = enemy.transform.position;

        rb = GetComponent<Rigidbody2D>();
    }

    public void FixedUpdate()
    {
        if(!isReturning && enemy != null) endPos = ((Vector2)enemy.transform.position - rb.position).normalized;
        rb.MovePosition(rb.position + endPos * moveSpeed * Time.fixedDeltaTime);

        if (isReturning)
        {
            if (transform.position.x < -0.5) Destroy(gameObject);
            if (transform.position.y < -0.5) Destroy(gameObject);
            if (transform.position.x > 1.5) Destroy(gameObject);
            if (transform.position.y > 1.5) Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == enemy)
        {
            endPos = startPos;
            isReturning = true;
            other.GetComponent<Enemy>()?.TakeDamage(10);
        }
    }
}
