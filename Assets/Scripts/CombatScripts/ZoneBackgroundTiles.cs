using System;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Zone Background Tiles")]
public class ZoneBackgroundTiles : ScriptableObject
{
    [Serializable]
    public struct TileEntry
    {
        public TileBase tile;

        [SerializeField] public int minClusterCount;
        [SerializeField] public int maxClusterCount;

        [Min(1)]
        [SerializeField] public int minClusterRadius;
        [Min(1)]
        [SerializeField] public int maxClusterRadius;
    }

    public TileBase[] baseTile;
    public TileEntry[] clusterRuleTiles;
}
