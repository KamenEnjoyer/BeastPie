
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ZoneClustersGeneration : MonoBehaviour
{
    [SerializeField] private Tilemap backgroundTilemap;
    [SerializeField] private Tilemap clusterTilemap;

    [Min(0)]
    [SerializeField] private int padding = 2;

    [Min(0.01f)]
    [SerializeField] private float boundaryNoiseScale = 0.15f;

    private Camera targetCamera;

    private Vector3Int minCell;
    private Vector3Int maxCell;

    public static ZoneClustersGeneration Instance;
    private void Awake()
    {
        Instance = this;
    }

    public void GetVisibleCellBounds()
    {
        targetCamera = Camera.main;
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

    public Vector3Int[] GetCellBounds()
    {
        return new Vector3Int[] {minCell, maxCell};
    }


    public void GenerateBackground(ZoneBackgroundTiles tilesSet)
    {
        if (tilesSet.baseTiles == null || tilesSet.baseTiles.Count == 0)
        {
            Debug.LogError("Base Tiles are not assigned.", this);
            return;
        }

        GenerateBase(tilesSet.baseTiles);
        GenerateClusters(tilesSet.clusterRuleTiles);
    }

    private void GenerateBase(List<ZoneBackgroundTiles.TileBaseEntry> baseTiles)
    {
        backgroundTilemap.ClearAllTiles();

        float totalWeight = 0;
        for (int i = 0; i < baseTiles.Count; i++)
        {
            totalWeight += baseTiles[i].spawnWeight;
        }

        for (int x = minCell.x; x <= maxCell.x; x++)
        {
            for (int y = minCell.y; y <= maxCell.y; y++)
            {
                backgroundTilemap.SetTile(
                    new Vector3Int(x, y, 0),
                    ChooseTile(baseTiles, totalWeight)
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

    private void GenerateClusters(ZoneBackgroundTiles.TileEntry[] clusterRuleTiles)
    {
        clusterTilemap.ClearAllTiles();

        if (clusterRuleTiles == null || clusterRuleTiles.Length == 0) return;

        foreach (var entry in clusterRuleTiles)
        {
            int clusterCount = Random.Range(entry.minClusterCount, entry.maxClusterCount + 1);
            if (clusterCount <= 0) continue;
            for (int i = 0; i < clusterCount; i++)
            {
                GenerateOneCluster(entry.tile, entry.minClusterRadius, entry.maxClusterRadius, clusterTilemap);
            }
            clusterTilemap.RefreshAllTiles();
        }
    }

    public void GenerateOneCluster(TileBase tile, int minClusterRadius, int maxClusterRadius, Tilemap tilemap)
    {
        int centerX = Random.Range(minCell.x, maxCell.x + 1);
        int centerY = Random.Range(minCell.y, maxCell.y + 1);

        int radius = Random.Range(minClusterRadius, maxClusterRadius + 1);

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
                    tilemap.SetTile(new Vector3Int(x, y, 0), tile);
                }
            }
        }
    }
}
