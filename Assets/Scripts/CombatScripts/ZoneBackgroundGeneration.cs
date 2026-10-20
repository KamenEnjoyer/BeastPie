using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ZoneBackgroundGenerator : MonoBehaviour
{
    [Header("Background")]
    public Tilemap backgroundTilemap;
    public ZoneBackgroundTiles tileSet;

    [Header("Settings")]
    [Min(0)] public int padding = 1;

    public static ZoneBackgroundGenerator Instance;
    private void Awake()
    {
        Instance = this;
    }

    public void Generate()
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

        for (int x = minCell.x; x <= maxCell.x; x++)
        {
            for (int y = minCell.y; y <= maxCell.y; y++)
            {
                TileBase tile = ChooseTile(candidates, totalWeight);

                backgroundTilemap.SetTile(new Vector3Int(x, y, 0), tile);
            }
        }
    }

    private TileBase ChooseTile(
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
    }
}