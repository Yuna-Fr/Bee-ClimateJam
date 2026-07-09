#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

/// <summary>
/// Batch : Unity -batchmode -projectPath ... -executeMethod LevelDressingBatchBake.BakeForestScene -quit
/// </summary>
public static class LevelDressingBatchBake
{
    const string ScenePath = "Assets/Scenes/Level_01_Forest.unity";

    public static void BakeForestScene()
    {
        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            Debug.LogError("[LevelDressingBatchBake] Annulé — scènes modifiées non sauvegardées.");
            EditorApplication.Exit(1);
            return;
        }

        var scene = EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);
        if (!scene.IsValid())
        {
            Debug.LogError($"[LevelDressingBatchBake] Impossible d'ouvrir {ScenePath}");
            EditorApplication.Exit(1);
            return;
        }

        var spawner = Object.FindAnyObjectByType<LevelDressingSpawner>();
        if (spawner == null)
        {
            var go = new GameObject("LevelDressing");
            spawner = go.AddComponent<LevelDressingSpawner>();
        }

        WirePrefabs(spawner);
        SetCameraOrthoSize(8f);
        spawner.BuildLevel();

        var so = new SerializedObject(spawner);
        so.FindProperty("spawnOnAwake").boolValue = false;
        so.ApplyModifiedPropertiesWithoutUndo();

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        var childCount = spawner.transform.childCount;
        Debug.Log($"[LevelDressingBatchBake] OK — {childCount} objets enfants sous LevelDressing dans {ScenePath}");
        EditorApplication.Exit(0);
    }

    static void WirePrefabs(LevelDressingSpawner spawner)
    {
        var so = new SerializedObject(spawner);
        Assign(so, "beePrefab", "Assets/Prefabs/P_Bee.prefab");
        Assign(so, "flowerPrefab", "Assets/Prefabs/Objects/Flowers/P_Flower.prefab");
        Assign(so, "branchPrefab", "Assets/Prefabs/Objects/P_Obstacle_Branch.prefab");
        Assign(so, "hornetPrefab", "Assets/Prefabs/P_Hornet.prefab");
        Assign(so, "pesticidePrefab", "Assets/Prefabs/Objects/P_PesticideZone.prefab");
        Assign(so, "nectarPrefab", "Assets/Prefabs/Objects/P_PollenNectar.prefab");
        so.FindProperty("spawnOnAwake").boolValue = false;
        so.ApplyModifiedPropertiesWithoutUndo();
    }

    static void SetCameraOrthoSize(float size)
    {
        var cam = Camera.main;
        if (cam == null) return;
        cam.orthographicSize = size;
        EditorUtility.SetDirty(cam);
    }

    static void Assign(SerializedObject so, string prop, string assetPath)
    {
        var p = so.FindProperty(prop);
        if (p == null) return;
        p.objectReferenceValue = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
    }
}
#endif