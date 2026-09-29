using System;
using UnityEngine;

[CreateAssetMenu(menuName = "ZoneType")]
public class ZoneType : ScriptableObject
{
    [Serializable]
    public struct Obstacle
    {
        public GameObject prefab;
        public float spawnChance;
    }
    [Serializable]
    public struct Enemies
    {
        public EnemyTypeAsset enemyType;
        public float spawnChance;
    }

    public string id;

    public enum ZoneName { Reality, Chitin, Slime, Flesh, Plant, Fire, Water, Earth, Storm, Void};
    public ZoneName zoneType;

    public Vector2 mapPosition;

    [Header("Spawn")]
    public Enemies[] enemies;
    public int minEnemies = 1;
    public int maxEnemies = 5;
    public float minEnemySpawnInterval = 2f;
    public float maxEnemySpawnInterval = 2f;
    
    public Obstacle[] obstacles;
    public int minObstacles = 0;
    public int maxObstacles = 5;
}
