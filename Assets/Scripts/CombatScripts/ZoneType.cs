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

    public enum ZoneName { Spruce_forest, Forest, Desert, Mountain, Swamp, Cave, Plains };
    public ZoneName zoneType;

    public Sprite backgroundImage;
    //music, sounds

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
