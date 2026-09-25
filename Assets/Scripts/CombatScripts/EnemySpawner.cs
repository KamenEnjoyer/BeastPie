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

    private int GetEnemyIndex()
    {
        float totalChance = 0f;
        foreach (var enemy in enemies) totalChance += enemy.spawnChance;
        float randomValue = Random.Range(0f, totalChance);

        float currentChance = 0f;
        for (int i = 0; i < enemies.Length; i++)
        {
            currentChance += enemies[i].spawnChance;

            if (randomValue < currentChance) return i;
        }

        return enemies.Length - 1;
    }

    public void SpawnEnemy()
    {
        int index = GetEnemyIndex();

        Vector2 spawnPos = enemies[index].enemyType.behaviourPrefab.GetComponent<EnemyBehaviour>().GetRandomEdgePosition();
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
