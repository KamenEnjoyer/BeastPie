using System;
using UnityEngine;
using static ZoneType;

[CreateAssetMenu(menuName = "ObstacleType")]
public class ObstacleType : ScriptableObject
{
    public enum Shape
    {
        Single,
        Rectangle,
        Circle
    }

    [Serializable]
    public struct NeighborRule
    {
        public ObstacleType obstacle;

        [Min(0f)]
        public float weightMultiplier;
    }

    [Header("Visual")]
    public GameObject prefab;

    [Header("Shape")]
    public Shape shape = Shape.Single;
    [Min(1)]
    public int width = 1;
    [Min(1)]
    public int height = 1;
    [Min(0)]
    public int radius = 1;

    [Header("Movement")]
    public bool blocksGround = true;
    public bool blocksFlying = true;

    [Header("Neighbors")]
    public NeighborRule[] neighborRules;

    public float GetNeighborMultiplier(ObstacleType obstacle)
    {
        foreach (NeighborRule rule in neighborRules)
        {
            if (rule.obstacle == obstacle) return rule.weightMultiplier;
        }

        return 1f;
    }
}