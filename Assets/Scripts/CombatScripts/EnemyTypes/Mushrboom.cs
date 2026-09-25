using UnityEngine;

public class Mushrboom : EnemyBehaviour
{
    public GameObject aoePrefab;
    public float attackInterval = 5f;
    public float damage;

    private float timer;

    protected override void Awake()
    {
        enemy = GetComponentInParent<Enemy>();
        rb = GetComponentInParent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (rb == null) Debug.LogError("Enemy prefab has no Rigidbody2D!");
    }

    public override void OnUpdate()
    {
        if (!player) return;

        timer -= Time.deltaTime;
        if (timer > 0f) return;

        AOEAttack();
        timer = attackInterval;
    }

    private void AOEAttack()
    {
        Vector2 attackDirection = transform.up;
        Vector2 spawnOffset = Vector2.zero;

        Vector2 spawnPosition = (Vector2)transform.position + spawnOffset;
        GameObject slash = Instantiate(aoePrefab, spawnPosition, Quaternion.identity);

        slash.GetComponent<MelleDamage>().isPlayer = false;
        slash.GetComponent<MelleDamage>().damage = damage;

        Destroy(slash, 2f);
    }

    public override Vector2 GetRandomEdgePosition()
    {
        Camera mainCamera = Camera.main;
        Vector2 min = mainCamera.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 max = mainCamera.ViewportToWorldPoint(new Vector2(1, 1));

        return new Vector2(Random.Range(min.x, max.x), Random.Range(min.y, max.y));
    }
}
