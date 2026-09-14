using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;

    private float minSpawnInterval = 2f;
    private float maxSpawnInterval = 5f;
    private int maxEnemies = 13;
    private int minEnemies = 7;

    private int spawnedEnemyCount = 0;
    private int enemyCountToSpawn;
    private int deathEnemyCount = 0;
    private ZoneType.Enemies[] enemies;
    private Camera mainCamera;

    private List<GameObject> spawnedEnemies = new List<GameObject>();

    public static EnemySpawner Instance;
    private void Awake()
    {
        Instance = this;
    }

    public void Setup(int maxEnemies, int minEnemies, float minSpawnInterval, float maxSpawnInterval, ZoneType.Enemies[] enemies)
    {
        this.enemies = enemies;
        if (enemies.Length == 0) Debug.LogError("No EnemyTypeAsset found in Resources/EnemyTypes");

        this.maxEnemies = maxEnemies;
        this.minEnemies = minEnemies;
        this.minSpawnInterval = minSpawnInterval;
        this.maxSpawnInterval = maxSpawnInterval;

        StartSpawning();
    }

    private void StartSpawning()
    {
        mainCamera = Camera.main;
        enemyCountToSpawn = Random.Range(minEnemies, maxEnemies + 1);
        InvokeRepeating(nameof(SpawnEnemy), 1f, Random.Range(minSpawnInterval, maxSpawnInterval)); //Cделать куротину
    }

    public void SpawnEnemy()
    {
        int index = Random.Range(0, enemies.Length);

        Vector2 spawnPos = GetRandomEdgePosition();
        GameObject enemyGO = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        enemyGO.GetComponent<Enemy>().Initialize(enemies[index].enemyType);
        spawnedEnemies.Add(enemyGO);
        spawnedEnemyCount++;
        Debug.Log($"Spawned enemy {spawnedEnemyCount}/{enemyCountToSpawn}");
        if (spawnedEnemyCount >= enemyCountToSpawn)
        {
            CancelInvoke(nameof(SpawnEnemy));
        }
    }

    private Vector2 GetRandomEdgePosition()
    {
        Vector2 min = mainCamera.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 max = mainCamera.ViewportToWorldPoint(new Vector2(1, 1));

        int side = Random.Range(0, 4);

        switch (side)
        {
            case 0: return new Vector2(min.x, Random.Range(min.y, max.y)); // left
            case 1: return new Vector2(max.x, Random.Range(min.y, max.y)); // right
            case 2: return new Vector2(Random.Range(min.x, max.x), max.y); // top
            default: return new Vector2(Random.Range(min.x, max.x), min.y); // bottom
        }
    }

    public void isItVictory(Enemy enemy)
    {
        spawnedEnemies.Remove(enemy.gameObject);
        deathEnemyCount++;
        if (deathEnemyCount >= enemyCountToSpawn)
        {
            MenuSpawner.Instance.ShowVictoryMenu();
        }
    }

    public GameObject GetClosestEnemy()
    {
        GameObject closest = null;
        Vector2 center = mainCamera.ViewportToWorldPoint(new Vector2(0.5f, 0.5f));
        float maxDist = 0f;
        foreach (var enemy in spawnedEnemies)
        {
            if (enemy == null) continue;
            float dist = Vector2.Distance(center, enemy.transform.position);
            if (dist > maxDist)
            {
                maxDist = dist;
                closest = enemy;
            }
        }
        return closest;
    }
}
