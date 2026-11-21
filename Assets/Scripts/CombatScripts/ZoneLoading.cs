using UnityEngine;

public class ZoneLoading : MonoBehaviour
{
    private ZoneType currentZone;

    public static ZoneLoading Instance;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        currentZone = ZoneTransfer.zone;

        ZoneClustersGeneration.Instance?.GetVisibleCellBounds();
        ZoneClustersGeneration.Instance?.GenerateBackground(currentZone.backgroundTileSet);
        int rand = Random.Range(currentZone.minObstacles, currentZone.maxObstacles + 1);
        ObstacleGeneration.Instance?.GenerateObstacles(currentZone.obstacles, rand < 0 ? 0 : rand);

        EnemySpawner.Instance.Setup(
            currentZone.minEnemies,
            currentZone.maxEnemies,
            currentZone.maxEnemySpawnInterval,
            currentZone.minEnemySpawnInterval,
            currentZone.enemies
        );
    }
}
