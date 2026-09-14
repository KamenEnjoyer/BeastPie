using UnityEngine;

public class PlayerTigerSpawner : MonoBehaviour
{
    public GameObject tigerPrefab;
    public float spawnInterval = 6f;

    private Camera mainCamera;

    public static PlayerTigerSpawner Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        mainCamera = Camera.main;
        InvokeRepeating(nameof(SpawnTiger), spawnInterval, spawnInterval);
    }

    public void SpawnTiger()
    {
        Vector2 spawnPos = GetStartPosition();

        GameObject tigerGO = Instantiate(tigerPrefab, spawnPos, Quaternion.identity);
        tigerGO.GetComponent<PlayerTiger>().Initialize(EnemySpawner.Instance.GetClosestEnemy());
    }

    private Vector2 GetStartPosition()
    {
        Vector2 min = mainCamera.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 max = mainCamera.ViewportToWorldPoint(new Vector2(1, 1));
        Vector2 enemyposition;
        try
        {
            enemyposition = EnemySpawner.Instance.GetClosestEnemy().transform.position;
        }
        catch (System.NullReferenceException)
        {
            Debug.LogWarning("Tiger can't found enemy!");
            return min;
        }

        Vector2 top = new Vector2(min.x, enemyposition.y);
        Vector2 bottom = new Vector2(max.x, enemyposition.y);
        Vector2 left = new Vector2(enemyposition.x, min.y);
        Vector2 right = new Vector2(enemyposition.x, max.y);

        Vector2 startPosition = top;
        if (Vector2.Distance(startPosition, enemyposition) > Vector2.Distance(bottom, enemyposition)) startPosition = bottom;
        if (Vector2.Distance(startPosition, enemyposition) > Vector2.Distance(left, enemyposition)) startPosition = left;
        if (Vector2.Distance(startPosition, enemyposition) > Vector2.Distance(right, enemyposition)) startPosition = right;

        return startPosition;
    }
}
