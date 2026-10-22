using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ZoneBackgroundGenerator : MonoBehaviour
{
    [Header("Background")]
    public Tilemap backgroundTilemap;

    [Header("Settings")]
    [Min(0)] public int padding = 1;

    [Min(0)]
    public int minClusterCount = 2;
    [Min(0)]
    public int maxClusterCount = 7;

    [Min(1)]
    public int minClusterRadius = 2;
    [Min(1)]
    public int maxClusterRadius = 5;

    [Min(0.01f)]
    public float boundaryNoiseScale = 0.25f;

    public static ZoneBackgroundGenerator Instance;
    private void Awake()
    {
        Instance = this;
    }

    public void Generate(ZoneBackgroundTiles tileSet)
    {
        if (backgroundTilemap == null || tileSet == null)
        {
            Debug.LogError("BackgroundGenerator: Tilemap or TileSet is missing.", this);
            return;
        }

        Camera targetCamera = Camera.main;
        if (targetCamera == null)
        {
            Debug.LogError("BackgroundGenerator: Camera is missing.", this);
            return;
        }

        if (tileSet.tiles == null || tileSet.tiles.Length == 0)
        {
            Debug.LogError("BackgroundGenerator: TileSet is empty.", this);
            return;
        }

        List<ZoneBackgroundTiles.TileEntry> candidates = new();
        float totalWeight = 0f;

        foreach (var entry in tileSet.tiles)
        {
            if (entry.tile == null || entry.spawnWeight <= 0f)
                continue;

            candidates.Add(entry);
            totalWeight += entry.spawnWeight;
        }

        if (candidates.Count == 0)
        {
            Debug.LogError("BackgroundGenerator: No valid tiles.", this);
            return;
        }

        // Расстояние от камеры до плоскости Tilemap.
        float distance = Vector3.Dot(
            backgroundTilemap.transform.position - targetCamera.transform.position,
            targetCamera.transform.forward
        );

        if (distance <= 0f)
        {
            Debug.LogError("BackgroundGenerator: Tilemap is behind the camera.", this);
            return;
        }

        // Углы видимой области камеры в мировых координатах.
        Vector3 bottomLeft = targetCamera.ViewportToWorldPoint(new Vector3(0f, 0f, distance));

        Vector3 topRight = targetCamera.ViewportToWorldPoint(new Vector3(1f, 1f, distance));

        // Переводим мировые координаты в клетки Tilemap.
        Vector3Int minCell = backgroundTilemap.WorldToCell(bottomLeft);
        Vector3Int maxCell = backgroundTilemap.WorldToCell(topRight);

        minCell.x -= padding;
        minCell.y -= padding;
        maxCell.x += padding;
        maxCell.y += padding;

        backgroundTilemap.ClearAllTiles();

        GenerateClusteredBackground(minCell, maxCell, Random.Range(minClusterCount, maxClusterCount + 1), tileSet);

        /*for (int x = minCell.x; x <= maxCell.x; x++)
        {
            for (int y = minCell.y; y <= maxCell.y; y++)
            {
                TileBase tile = ChooseTile(candidates, totalWeight);

                backgroundTilemap.SetTile(new Vector3Int(x, y, 0), tile);
            }
        }*/
    }

    private void GenerateClusteredBackground(Vector3Int minCell, Vector3Int maxCell, int clusterCount, ZoneBackgroundTiles tileSet)
    {
        if (tileSet.baseTile == null)
        {
            Debug.LogError("Base Tile is not assigned.", this);
            return;
        }

        backgroundTilemap.ClearAllTiles();
        for (int x = minCell.x; x <= maxCell.x; x++)
        {
            for (int y = minCell.y; y <= maxCell.y; y++)
            {
                backgroundTilemap.SetTile(
                    new Vector3Int(x, y, 0),
                    tileSet.baseTile
                );
            }
        }

        if (tileSet.clusterRuleTiles == null || tileSet.clusterRuleTiles.Length == 0) return;

        for (int i = 0; i < clusterCount; i++)
        {
            TileBase clusterTile = tileSet.clusterRuleTiles[Random.Range(0, tileSet.clusterRuleTiles.Length)];

            if (clusterTile == null) continue;

            int centerX = Random.Range(minCell.x, maxCell.x + 1);
            int centerY = Random.Range(minCell.y, maxCell.y + 1);

            int radius = Random.Range(minClusterRadius, maxClusterRadius + 1);

            float offsetX = Random.Range(0f, 1000f);
            float offsetY = Random.Range(0f, 1000f);

            int extent = Mathf.CeilToInt(radius * 1.15f);

            for (int x = centerX - extent; x <= centerX + extent; x++)
            {
                for (int y = centerY - extent; y <= centerY + extent; y++)
                {
                    if (x < minCell.x || x > maxCell.x ||
                        y < minCell.y || y > maxCell.y) continue;

                    float distance = Vector2.Distance(
                        new Vector2(x, y),
                        new Vector2(centerX, centerY)
                    );

                    // Плавно меняем границу пятна с помощью шума.
                    float noise = Mathf.PerlinNoise(
                        (x + offsetX) * boundaryNoiseScale,
                        (y + offsetY) * boundaryNoiseScale
                    );

                    float localRadius = radius * Mathf.Lerp(0.65f, 1.15f, noise);

                    if (distance <= localRadius)
                    {
                        backgroundTilemap.SetTile(new Vector3Int(x, y, 0), clusterTile);
                    }
                }
            }
        }

        // Пересчитываем визуальные правила после размещения кластеров.
        backgroundTilemap.RefreshAllTiles();
    }

    /*private TileBase ChooseTile(
        List<ZoneBackgroundTiles.TileEntry> candidates,
        float totalWeight)
    {
        float roll = Random.value * totalWeight;

        foreach (var entry in candidates)
        {
            roll -= entry.spawnWeight;

            if (roll <= 0f) return entry.tile;
        }

        return candidates[candidates.Count - 1].tile;
    }*/
}