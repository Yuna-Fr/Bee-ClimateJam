#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

/// <summary>
/// Place 2 Wind prefabs manually in Level_01_Forest without rebaking LevelDressing.
/// Batch: -executeMethod LevelDressingPlaceWind.PlaceWindZones
/// </summary>
public static class LevelDressingPlaceWind
{
    const string ScenePath = "Assets/Scenes/Level_01_Forest.unity";
    const string WindPrefabPath = "Assets/Prefabs/Objects/Wind.prefab";

    public static void PlaceWindZones()
    {
        var scene = EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);
        if (!scene.IsValid())
        {
            Debug.LogError($"[PlaceWind] Cannot open {ScenePath}");
            EditorApplication.Exit(1);
            return;
        }

        var windPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(WindPrefabPath);
        if (windPrefab == null)
        {
            Debug.LogError($"[PlaceWind] Missing prefab at {WindPrefabPath}");
            EditorApplication.Exit(1);
            return;
        }

        var root = GameObject.Find("LevelDressing")?.transform;
        if (root == null)
        {
            Debug.LogError("[PlaceWind] LevelDressing not found in scene.");
            EditorApplication.Exit(1);
            return;
        }

        RemovePlaced(root, "Wind_WZ");
        RemovePlaced(root, "Pesticide_WindEnd_");

        PlaceOne(root, windPrefab, "Wind_WZ01", new Vector3(65f, 0f, 0f), new Vector3(2.5f, 5f, 1f), 12f);
        PlaceOne(root, windPrefab, "Wind_WZ02", new Vector3(250f, -2.5f, 0f), new Vector3(2.5f, 5f, 1f), 12f);

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
        Debug.Log("[PlaceWind] OK — 2 Wind zones placed (65m, 250m), windStrength=12.");
        EditorApplication.Exit(0);
    }

    static void RemovePlaced(Transform root, string prefix)
    {
        for (int i = root.childCount - 1; i >= 0; i--)
        {
            var child = root.GetChild(i);
            if (child.name.StartsWith(prefix))
                Object.DestroyImmediate(child.gameObject);
        }
    }

    static void PlaceOne(Transform root, GameObject prefab, string id, Vector3 pos, Vector3 scale, float strength)
    {
        var go = (GameObject)PrefabUtility.InstantiatePrefab(prefab, root);
        go.name = id;
        go.transform.position = pos;
        go.transform.localScale = scale;

        var wind = go.GetComponent<Wind>();
        if (wind != null)
        {
            var so = new SerializedObject(wind);
            so.FindProperty("windStrength").floatValue = strength;
            so.ApplyModifiedPropertiesWithoutUndo();
        }
    }
}
#endif