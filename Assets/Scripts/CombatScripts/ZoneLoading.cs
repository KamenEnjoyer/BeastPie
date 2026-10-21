using UnityEngine;
using UnityEngine.UI;

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
        //background.sprite = Resources.Load<Sprite>("ZonesBackgrounds/" + currentZone.id + "_background");

        ZoneBackgroundGenerator.Instance.Generate(currentZone.backgroundTileSet);

        EnemySpawner.Instance.Setup(
            currentZone.minEnemies,
            currentZone.maxEnemies,
            currentZone.maxEnemySpawnInterval,
            currentZone.minEnemySpawnInterval,
            currentZone.enemies
        );
    }
}
