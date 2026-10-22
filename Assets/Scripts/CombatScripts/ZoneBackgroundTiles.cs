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
        [Min(0f)]
        public float spawnWeight;
    }

    public TileEntry[] tiles;


    public TileBase baseTile;
    public TileBase[] clusterRuleTiles;
}
