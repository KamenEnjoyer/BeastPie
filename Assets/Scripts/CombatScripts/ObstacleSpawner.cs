using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    private int minObjectsCount = 0;
    private int maxObjectsCount = 5;

    private Camera mainCamera;

    private ZoneType.Obstacle[] obstacles;

    public static ObstacleSpawner Instance;
    private void Awake()
    {
        Instance = this;
    }

    public void Setup(int minObjectsCount, int maxObjectsCount, ZoneType.Obstacle[] obstacles)
    {
        this.minObjectsCount = minObjectsCount;
        this.maxObjectsCount = maxObjectsCount;
        this.obstacles = obstacles;

        GenerateObstacles();
    }

    private void GenerateObstacles()
    {
        mainCamera = Camera.main;
        Vector2 min = mainCamera.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 max = mainCamera.ViewportToWorldPoint(new Vector2(1, 1));

        int spawned = 0;
        int objectsCount = Random.Range(minObjectsCount, maxObjectsCount + 1);

        while (spawned < objectsCount)
        {
            ZoneType.Obstacle obstacle = obstacles[Random.Range(0, obstacles.Length)];

            Vector2 position = new Vector2(
                Random.Range(min.x, max.x),
                Random.Range(min.y, max.y)
            );

            if (CanSpawn(obstacle.prefab, position))
            {
                Instantiate(obstacle.prefab, position, Quaternion.identity, transform);
                spawned++;
            }
        }
    }

    private bool CanSpawn(GameObject prefab, Vector2 position)
    {
        Collider2D col = prefab.GetComponent<Collider2D>();
        if (!col) return true;

        Vector2 size = col.bounds.size;

        Collider2D hit = Physics2D.OverlapBox(position, size, 0f);

        return hit == null;
    }
}
