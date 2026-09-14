using UnityEngine;
using UnityEngine.UI;

public class ZoneLoading : MonoBehaviour
{
    public Image background;

    private ZoneType[] zoneTypes;
    private ZoneType currentZone;

    public static ZoneLoading Instance;

    private void Awake()
    {
        Instance = this;

        zoneTypes = Resources.LoadAll<ZoneType>("ZoneTypes");
        if (zoneTypes.Length == 0) Debug.LogError("No ZoneType found in Resources/ZoneTypes");
    }

    private void Start()
    {
        int index = Random.Range(0, zoneTypes.Length);
        currentZone = zoneTypes[index];
        background.sprite = currentZone.backgroundImage;

        ObstacleSpawner.Instance.Setup(currentZone.minObstacles, currentZone.maxObstacles, currentZone.obstacles);

        EnemySpawner.Instance.Setup(
            currentZone.minEnemies,
            currentZone.maxEnemies,
            currentZone.maxEnemySpawnInterval,
            currentZone.minEnemySpawnInterval,
            currentZone.enemies
        );
    }
}
