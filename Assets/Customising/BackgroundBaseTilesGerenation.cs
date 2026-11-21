#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

[CustomEditor(typeof(ZoneBackgroundTiles))]
public class ZoneBackgroundTilesEditor : Editor
{
    private DefaultAsset sourceFolder;

    private void OnEnable()
    {
        string assetPath = AssetDatabase.GetAssetPath(target);
        string folderPath = System.IO.Path.GetDirectoryName(assetPath) + "/Base";

        sourceFolder = AssetDatabase.LoadAssetAtPath<DefaultAsset>(folderPath);
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField(
            "Automatic Base Tiles Setup",
            EditorStyles.boldLabel
        );

        sourceFolder = (DefaultAsset)EditorGUILayout.ObjectField(
            "Tiles Folder",
            sourceFolder,
            typeof(DefaultAsset),
            false
        );

        if (GUILayout.Button("Generate Base Tiles List"))
        {
            GenerateTiles();
        }
    }

    private void GenerateTiles()
    {
        if (sourceFolder == null)
        {
            Debug.LogWarning("Select a folder containing TileBase assets.");
            return;
        }

        string folderPath = AssetDatabase.GetAssetPath(sourceFolder);

        if (!AssetDatabase.IsValidFolder(folderPath))
        {
            Debug.LogWarning("Selected asset is not a folder.");
            return;
        }

        ZoneBackgroundTiles data = (ZoneBackgroundTiles)target;

        string[] guids = AssetDatabase.FindAssets(
            "t:TileBase",
            new[] { folderPath }
        );

        var tiles = new List<ZoneBackgroundTiles.TileBaseEntry>();

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);

            TileBase tile = AssetDatabase.LoadAssetAtPath<TileBase>(path);

            if (tile == null) continue;

            tiles.Add(new ZoneBackgroundTiles.TileBaseEntry
            {
                tile = tile,
                spawnWeight = 1f
            });
        }

        Undo.RecordObject(data, "Generate Base Tiles List");

        data.baseTiles = tiles;

        EditorUtility.SetDirty(data);
        AssetDatabase.SaveAssets();

        Debug.Log($"Added {tiles.Count} tiles to baseTiles.");
    }
}

#else

#endif