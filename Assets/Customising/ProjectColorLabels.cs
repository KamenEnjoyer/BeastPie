#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class ProjectColorLabels
{

    static ProjectColorLabels()
    {
        EditorApplication.projectWindowItemOnGUI += OnProjectItemGUI;
    }

    static void OnProjectItemGUI(string guid, Rect selectionRect)
    {

        string path = AssetDatabase.GUIDToAssetPath(guid);
        if (path.Contains("Inventory")) EditorGUI.DrawRect(selectionRect, new Color(0f / 255f, 215f / 255f, 45f / 255f, 0.2f));
        else if (path.Contains("Ingredient")) EditorGUI.DrawRect(selectionRect, new Color(15f / 255f, 140f / 255f, 230f / 255f, 0.2f));
        else if (path.Contains("Mix")) EditorGUI.DrawRect(selectionRect, new Color(140f / 255f, 30f / 255f, 155f / 255f, 0.2f));
        else if (path.Contains("Storage")) EditorGUI.DrawRect(selectionRect, new Color(1f, 215f / 255f, 0, 0.2f));
        else if (path.Contains("Menu")) EditorGUI.DrawRect(selectionRect, new Color(243f / 255f, 145f / 255f, 72f / 255f, 0.2f));
        else if (path.Contains("Potion")) EditorGUI.DrawRect(selectionRect, new Color(247f / 255f, 67f / 255f, 160f / 255f, 0.2f));
        else if (path.Contains("Player")) EditorGUI.DrawRect(selectionRect, new Color(10f / 255f, 67f / 255f, 215f / 255f, 0.2f));
        else if (path.Contains("Enemy")) EditorGUI.DrawRect(selectionRect, new Color(255f / 255f, 30f / 255f, 30f / 255f, 0.2f));
        else if (path.Contains("Effect")) EditorGUI.DrawRect(selectionRect, new Color(100f / 255f, 225f / 255f, 20f / 255f, 0.2f));
        else if (path.Contains("Obstacle")) EditorGUI.DrawRect(selectionRect, new Color(255f / 255f, 165f / 255f, 0f / 255f, 0.2f));
        else if (path.Contains("Zone")) EditorGUI.DrawRect(selectionRect, new Color(235f / 255f, 255f / 255f, 0f / 255f, 0.2f));

    }
}
#else
        
#endif