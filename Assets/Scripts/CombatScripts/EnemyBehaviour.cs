using UnityEngine;

public abstract class EnemyBehaviour : MonoBehaviour
{
    protected Enemy enemy;
    protected Rigidbody2D rb;
    protected Transform player;

    protected virtual void Awake()
    {
        enemy = GetComponentInParent<Enemy>();
        rb = GetComponentInParent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (rb == null) Debug.LogError("Enemy prefab has no Rigidbody2D!");
    }

    public virtual Vector2 GetRandomEdgePosition()
    {
        Camera mainCamera = Camera.main;
        Vector2 min = mainCamera.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 max = mainCamera.ViewportToWorldPoint(new Vector2(1, 1));

        int side = Random.Range(0, 4);

        switch (side)
        {
            case 0: return new Vector2(min.x, Random.Range(min.y, max.y)); // left
            case 1: return new Vector2(max.x, Random.Range(min.y, max.y)); // right
            case 2: return new Vector2(Random.Range(min.x, max.x), max.y); // top
            default: return new Vector2(Random.Range(min.x, max.x), min.y); // bottom
        }
    }

    public virtual void OnUpdate() { }
    public virtual void OnFixedUpdate() { }
}
