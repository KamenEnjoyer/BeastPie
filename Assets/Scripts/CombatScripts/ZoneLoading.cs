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
        
        ZoneBackgroundGeneration.Instance.Generate(currentZone.backgroundTileSet);

        EnemySpawner.Instance.Setup(
            currentZone.minEnemies,
            currentZone.maxEnemies,
            currentZone.maxEnemySpawnInterval,
            currentZone.minEnemySpawnInterval,
            currentZone.enemies
        );
    }
}
