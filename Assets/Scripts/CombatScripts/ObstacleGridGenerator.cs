using System.Collections.Generic;
using UnityEngine;

public class ObstacleGridGenerator : MonoBehaviour
{
    [Min(1)] public int width = 20;
    [Min(1)] public int height = 15; //переписать на то, чтобы подсчитывалось с учётом размеров экрана.
    [Min(0.1f)] public float cellSize = 1f;

    public Transform obstacleParent;


    private ObstacleType[,] cells;

    private readonly List<GameObject> spawnedObjects = new();

    public enum MovementType
    {
        Ground,
        Flying,
        Underground
    }

    public static ObstacleGridGenerator Instance;
    private void Awake()
    {
        Instance = this;
    }

    public void Generate(ZoneType zone)
    {
        foreach (GameObject obj in spawnedObjects)
        {
            if (obj != null) Destroy(obj);
        }
        spawnedObjects.Clear();

        cells = new ObstacleType[width, height];

        int obstacleCount = Random.Range(zone.minObstacles, zone.maxObstacles + 1);

        for (int i = 0; i < obstacleCount; i++)
        {
            if (zone.obstacles == null || zone.obstacles.Length == 0) break;

            Vector2Int anchor = new Vector2Int(
                Random.Range(0, width),
                Random.Range(0, height)
            );

            ObstacleType type = ChooseObstacle(anchor, zone.obstacles);

            if (type != null) TryPlaceObstacle(type, anchor);
        }
    }

    private ObstacleType ChooseObstacle(Vector2Int anchor, ZoneType.Obstacle[] obstacles)
    {
        List<ObstacleType> candidates = new();
        List<float> weights = new();

        float totalWeight = 0f;

        foreach (ZoneType.Obstacle entry in obstacles)
        {
            ObstacleType type = entry.type;

            if (type == null || entry.spawnChance <= 0f) continue;

            float weight = entry.spawnChance;

            // Учитываем соседние препятствия.
            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    if (dx == 0 && dy == 0) continue;

                    int x = anchor.x + dx;
                    int y = anchor.y + dy;

                    if (!IsInside(x, y)) continue;

                    ObstacleType neighbor = cells[x, y];

                    if (neighbor == null) continue;

                    weight *= type.GetNeighborMultiplier(neighbor);
                }
            }

            if (weight <= 0f) continue;

            candidates.Add(type);
            weights.Add(weight);
            totalWeight += weight;
        }

        if (totalWeight <= 0f) return null;

        float roll = Random.value * totalWeight;

        for (int i = 0; i < candidates.Count; i++)
        {
            roll -= weights[i];

            if (roll <= 0f) return candidates[i];
        }

        return candidates[^1];
    }

    private bool IsInside(int x, int y)
    {
        return x >= 0 && x < width &&
               y >= 0 && y < height;
    }

    private List<Vector2Int> GetFootprint(
        ObstacleType type,
        Vector2Int anchor)
    {
        List<Vector2Int> result = new();

        switch (type.shape)
        {
            case ObstacleType.Shape.Single:
                result.Add(anchor);
                break;

            case ObstacleType.Shape.Rectangle:
                for (int x = 0; x < type.width; x++)
                {
                    for (int y = 0; y < type.height; y++)
                    {
                        result.Add(anchor + new Vector2Int(x, y));
                    }
                }
                break;

            case ObstacleType.Shape.Circle:
                int radius = type.radius;

                for (int x = -radius; x <= radius; x++)
                {
                    for (int y = -radius; y <= radius; y++)
                    {
                        if (x * x + y * y <= radius * radius)
                        {
                            result.Add(
                                anchor + new Vector2Int(x, y)
                            );
                        }
                    }
                }
                break;
        }

        return result;
    }

    private bool TryPlaceObstacle(
        ObstacleType type,
        Vector2Int anchor)
    {
        if (type.prefab == null)
            return false;

        List<Vector2Int> footprint = GetFootprint(type, anchor);

        // Сначала проверяем все клетки.
        foreach (Vector2Int cell in footprint)
        {
            if (!IsInside(cell.x, cell.y))
                return false;

            if (cells[cell.x, cell.y] != null)
                return false;
        }

        // Только после полной проверки занимаем клетки.
        foreach (Vector2Int cell in footprint)
        {
            cells[cell.x, cell.y] = type;
        }

        Vector3 position = transform.position + new Vector3(
            (anchor.x + 0.5f) * cellSize,
            0f,
            (anchor.y + 0.5f) * cellSize
        );

        Transform parent = obstacleParent != null
            ? obstacleParent
            : transform;

        GameObject instance = Instantiate(
            type.prefab,
            position,
            Quaternion.identity,
            parent
        );

        spawnedObjects.Add(instance);

        return true;
    }

    public bool CanEnter(
        Vector2Int cell,
        MovementType movementType)
    {
        if (cells == null || !IsInside(cell.x, cell.y))
            return false;

        ObstacleType obstacle = cells[cell.x, cell.y];

        if (obstacle == null)
            return true;

        return movementType switch
        {
            MovementType.Ground => !obstacle.blocksGround,
            MovementType.Flying => !obstacle.blocksFlying,
            _ => false
        };
    }
}
