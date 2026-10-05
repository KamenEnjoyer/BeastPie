using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class ZoneLoading : MonoBehaviour
{
    public Image background;

    private ZoneType currentZone;

    public static ZoneLoading Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        currentZone = ZoneTransfer.zone;
        background.sprite = Resources.Load<Sprite>("ZonesBackgrounds/" + currentZone.id + "_background");

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
