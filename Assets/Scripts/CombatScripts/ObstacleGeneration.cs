using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ObstacleGeneration : MonoBehaviour
{
    [SerializeField] private Tilemap obstaclesTilemap;
    [SerializeField] private Transform obstaclesParent;

    private readonly List<GameObject> spawnedObjects = new();

    public static ObstacleGeneration Instance;
    private void Awake()
    {
        Instance = this;
    }

    public void GenerateObstacles(ObstacleType[] obstacleTypes, int count)
    {
        float totalWeight = 0;
        for (int i = 0; i < obstacleTypes.Length; i++)
        {
            totalWeight += obstacleTypes[i].spawnWeight;
        }

        List<ObstacleType> choosenObstacles = new();
        for (int i = 0; i < count; i++)
        {
            ObstacleType type = ChooseObstacle(obstacleTypes, totalWeight);
            choosenObstacles.Add(type);
        }
        choosenObstacles = choosenObstacles.OrderBy(x => x.generationType == ObstacleType.GenerationType.Cluster ? 0 : 1).ToList();

        foreach (var type in choosenObstacles)
        {
            Debug.Log($"Generating obstacle: {type.name}, GenerationType: {type.generationType}");
            if (type.generationType == ObstacleType.GenerationType.Cluster)
            {
                ZoneClustersGeneration.Instance.GenerateOneCluster(type.clusterRuleTiles.tile,
                                                                type.clusterRuleTiles.minClusterRadius,
                                                                type.clusterRuleTiles.maxClusterRadius,
                                                                obstaclesTilemap);
            }
            else if (type.generationType == ObstacleType.GenerationType.Object)
            {
                bool success = GenerateObjects(type.prefab, type.footprint);
                if (!success)
                {
                    Debug.LogWarning($"Failed to generate object {type.prefab.name}.");
                }
            }
        }
        /*
        int clusteredCount = Random.Range(0, count + 1);
        int objectCount = count - clusteredCount;
        while (clusteredCount > 0)
        {
            ObstacleType type = ChooseObstacle(obstacleTypes, totalWeight);
            if (type.generationType == ObstacleType.GenerationType.Cluster)
            {
                ZoneClustersGeneration.Instance.GenerateOneCluster(type.clusterRuleTiles.tile, 
                                                                type.clusterRuleTiles.minClusterRadius, 
                                                                type.clusterRuleTiles.maxClusterRadius, 
                                                                obstaclesTilemap);
                clusteredCount--;
            }
        }
        while (objectCount > 0)
        {
            ObstacleType type = ChooseObstacle(obstacleTypes, totalWeight);
            if (type.generationType == ObstacleType.GenerationType.Object)
            {
                bool success = GenerateObjects(type.prefab, type.footprint);
                if (success)
                {
                    objectCount--;
                }
            }
        }*/
        obstaclesTilemap.RefreshAllTiles();
    }

    private ObstacleType ChooseObstacle(ObstacleType[] candidates, float totalWeight)
    {
        float roll = Random.value * totalWeight;

        foreach (var entry in candidates)
        {
            roll -= entry.spawnWeight;

            if (roll <= 0f) return entry;
        }

        return candidates[candidates.Length - 1];
    }


    private bool GenerateObjects(GameObject obstPref, Vector2Int footprint)
    {
        for (int i = obstaclesParent.childCount - 1; i >= 0; i--)
        {
            Destroy(obstaclesParent.GetChild(i).gameObject);
        }

        HashSet<Vector3Int> occupiedCells = new();

        Vector3Int minCell = ZoneClustersGeneration.Instance.GetCellBounds()[0];
        Vector3Int maxCell = ZoneClustersGeneration.Instance.GetCellBounds()[1];
        int x = Random.Range(
            minCell.x,
            maxCell.x - footprint.x + 2
        );
        int y = Random.Range(
            minCell.y,
            maxCell.y - footprint.y + 2
        );

        Vector3Int anchorCell = new Vector3Int(x, y, 0);

        if (!IsAreaAvailable(anchorCell, footprint, occupiedCells))
        {
            return false;
        }

        Vector3 position = obstaclesTilemap.GetCellCenterWorld(anchorCell);

        Instantiate(obstPref, position, Quaternion.identity, obstaclesParent);

        for (int offsetX = 0; offsetX < footprint.x; offsetX++)
        {
            for (int offsetY = 0; offsetY < footprint.y; offsetY++)
            {
                occupiedCells.Add(new Vector3Int(x + offsetX, y + offsetY, 0));
            }
        }
        return true;
    }

    private bool IsAreaAvailable(Vector3Int anchorCell, Vector2Int footprint, HashSet<Vector3Int> occupiedCells)
    {
        for (int x = 0; x < footprint.x; x++)
        {
            for (int y = 0; y < footprint.y; y++)
            {
                Vector3Int cell = new Vector3Int(
                    anchorCell.x + x,
                    anchorCell.y + y,
                    0
                );

                // Проверяем, занята ли клетка другим объектом.
                if (occupiedCells.Contains(cell)) return false;

                // Получаем мировую позицию клетки.
                Vector3 worldPosition = obstaclesTilemap.GetCellCenterWorld(cell);

                // Переводим её в координаты Tilemap кластеров.
                Vector3Int clusterCell = obstaclesTilemap.WorldToCell(worldPosition);

                // Проверяем наличие кластера.
                if (obstaclesTilemap.HasTile(clusterCell)) return false;
            }
        }

        return true;
    }

}
