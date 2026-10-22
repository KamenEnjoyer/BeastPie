
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ZoneBackgroundGeneration : MonoBehaviour
{
    [SerializeField] private Tilemap backgroundTilemap;
    [SerializeField] private Tilemap clusterTilemap;

    [Min(0)]
    [SerializeField] private int padding = 2;

    [Min(0.01f)]
    [SerializeField] private float boundaryNoiseScale = 0.15f;

    private Camera targetCamera;

    public static ZoneBackgroundGeneration Instance;
    private void Awake()
    {
        Instance = this;
    }

    public void Generate(ZoneBackgroundTiles tilesSet)
    {
        if (backgroundTilemap == null || clusterTilemap == null)
        {
            Debug.LogError("Assign both background and cluster Tilemaps.", this);
            return;
        }
        if (tilesSet.baseTiles == null || tilesSet.baseTiles.Count == 0)
        {
            Debug.LogError("Base Tiles are not assigned.", this);
            return;
        }

        targetCamera = Camera.main;

        GetVisibleCellBounds(out Vector3Int minCell, out Vector3Int maxCell);

        GenerateBase(minCell, maxCell, tilesSet);
        GenerateClusters(minCell, maxCell, tilesSet);
    }

    private void GetVisibleCellBounds(out Vector3Int minCell, out Vector3Int maxCell)
    {
        float distance = Vector3.Dot(
            backgroundTilemap.transform.position - targetCamera.transform.position,
            targetCamera.transform.forward
        );

        Vector3 bottomLeft = targetCamera.ViewportToWorldPoint(
            new Vector3(0f, 0f, distance)
        );

        Vector3 topRight = targetCamera.ViewportToWorldPoint(
            new Vector3(1f, 1f, distance)
        );

        minCell = backgroundTilemap.WorldToCell(bottomLeft);
        maxCell = backgroundTilemap.WorldToCell(topRight);

        minCell.x -= padding;
        minCell.y -= padding;

        maxCell.x += padding;
        maxCell.y += padding;
    }

    private void GenerateBase(Vector3Int minCell, Vector3Int maxCell, ZoneBackgroundTiles tilesSet)
    {
        backgroundTilemap.ClearAllTiles();

        float totalWeight = 0;
        for (int i = 0; i < tilesSet.baseTiles.Count; i++)
        {
            totalWeight += tilesSet.baseTiles[i].spawnWeight;
        }

        for (int x = minCell.x; x <= maxCell.x; x++)
        {
            for (int y = minCell.y; y <= maxCell.y; y++)
            {
                backgroundTilemap.SetTile(
                    new Vector3Int(x, y, 0),
                    ChooseTile(tilesSet.baseTiles, totalWeight)
                );
            }
        }

        backgroundTilemap.RefreshAllTiles();
    }

    private TileBase ChooseTile(List<ZoneBackgroundTiles.TileBaseEntry> candidates, float totalWeight)
    {
        float roll = Random.value * totalWeight;

        foreach (var entry in candidates)
        {
            roll -= entry.spawnWeight;

            if (roll <= 0f) return entry.tile;
        }

        return candidates[candidates.Count - 1].tile;
    }

    private void GenerateClusters(Vector3Int minCell, Vector3Int maxCell, ZoneBackgroundTiles tilesSet)
    {
        clusterTilemap.ClearAllTiles();

        if (tilesSet.clusterRuleTiles == null || tilesSet.clusterRuleTiles.Length == 0) return;

        foreach (var entry in tilesSet.clusterRuleTiles)
        {
            int clusterCount = Random.Range(entry.minClusterCount, entry.maxClusterCount + 1);
            if (clusterCount <= 0) continue;
            for (int i = 0; i < clusterCount; i++)
            {
                int centerX = Random.Range(minCell.x, maxCell.x + 1);
                int centerY = Random.Range(minCell.y, maxCell.y + 1);

                int radius = Random.Range(entry.minClusterRadius, entry.maxClusterRadius + 1);

                float offsetX = Random.Range(0f, 1000f);
                float offsetY = Random.Range(0f, 1000f);

                int extent = Mathf.CeilToInt(radius * 1.2f);

                for (int x = centerX - extent; x <= centerX + extent; x++)
                {
                    for (int y = centerY - extent; y <= centerY + extent; y++)
                    {
                        if (x < minCell.x || x > maxCell.x ||
                            y < minCell.y || y > maxCell.y)
                            continue;

                        float distance = Vector2.Distance(
                            new Vector2(x, y),
                            new Vector2(centerX, centerY)
                        );

                        float noise = Mathf.PerlinNoise(
                            (x + offsetX) * boundaryNoiseScale,
                            (y + offsetY) * boundaryNoiseScale
                        );

                        float localRadius = radius * Mathf.Lerp(0.65f, 1.2f, noise);

                        if (distance <= localRadius)
                        {
                            // Поздние кластеры могут перекрывать ранние.
                            clusterTilemap.SetTile(new Vector3Int(x, y, 0), entry.tile);
                        }
                    }
                }
            }
            clusterTilemap.RefreshAllTiles();
        }
    }
}
