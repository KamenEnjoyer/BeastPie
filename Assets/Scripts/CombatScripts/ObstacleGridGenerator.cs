using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObstacleGridGenerator : MonoBehaviour
{
    [Min(0.1f)] public float cellSize = 1f;
    public Transform obstacleParent;

    private ObstacleType[,] cells;
    private int width;
    private int height;

    private readonly List<GameObject> spawnedObjects = new();

    public enum MovementType
    {
        Ground,
        Flying,
        Underground
    }

    public static ObstacleGridGenerator Instance;
    private void Awake()
    {
        Instance = this;
    }

    public void Generate(ZoneType zone)
    {
        
    }

}
