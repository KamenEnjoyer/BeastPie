using System;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "ObstacleType")]
public class ObstacleType : ScriptableObject
{
    public enum GenerationType
    {
        Cluster,
        Object
    }

    [Flags]
    public enum ObstacleBlocks
    {
        None = 0,
        Walking = 1 << 0,
        Flying = 1 << 1,
        Swimming = 1 << 2,
        All = Walking | Flying | Swimming
    }

    [Serializable]
    public struct TileEntry
    {
        public TileBase tile;

        [Min(1)]
        [SerializeField] public int minClusterRadius;
        [Min(1)]
        [SerializeField] public int maxClusterRadius;
    }

    [Header("Type")]
    public GenerationType generationType = GenerationType.Object;

    [Header("Object")]
    public GameObject prefab;
    public Vector2Int footprint = Vector2Int.one;

    public bool destructible;
    [Min(0)]
    public int health = 0;

    [Header("Cluster")]
    public TileEntry clusterRuleTiles;

    [Header("Properties")]
    public int spawnWeight = 1;


    public ObstacleBlocks blocks = ObstacleBlocks.All;
    public bool blocksProjectiles = true;
}