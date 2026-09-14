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

    public virtual void OnUpdate() { }
    public virtual void OnFixedUpdate() { }
}
