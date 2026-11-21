using System;
using UnityEngine;

[CreateAssetMenu(menuName = "ZoneType")]
public class ZoneType : ScriptableObject
{
    [Serializable]
    public struct Enemies
    {
        public EnemyTypeAsset enemyType;
        public float spawnChance;
    }

    public string id;

    public enum ZoneName { Reality, Chitin, Slime, Flesh, Plant, Fire, Water, Earth, Storm, Void};
    public ZoneName zoneType;

    [Header("Mapping")]
    public ZoneBackgroundTiles backgroundTileSet;

    public ObstacleType[] obstacles;
    public int minObstacles = 0;
    [Min(0f)]
    public int maxObstacles = 5;

    public Vector2 mapPosition;

    [Header("Spawn")]
    public Enemies[] enemies;
    [Min(0f)]
    public int minEnemies = 1;
    [Min(0f)]
    public int maxEnemies = 5;
    [Min(0f)]
    public float minEnemySpawnInterval = 2f;
    [Min(0f)]
    public float maxEnemySpawnInterval = 2f;
    
}
