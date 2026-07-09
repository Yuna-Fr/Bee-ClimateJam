#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class LevelDressingMenu
{
    const string MenuPath = "Bee Climate Jam/Build Level Dressing (0-500m)";

    [MenuItem(MenuPath)]
    static void BuildLevelDressing()
    {
        var spawner = Object.FindAnyObjectByType<LevelDressingSpawner>();
        if (spawner == null)
        {
            var go = new GameObject("LevelDressing");
            spawner = go.AddComponent<LevelDressingSpawner>();
            AutoWirePrefabs(spawner);
        }
        else
        {
            AutoWirePrefabs(spawner);
        }

        spawner.BuildLevel();
        EditorSceneManager.MarkSceneDirty(spawner.gameObject.scene);
        Debug.Log("Level dressing 0–500 m généré. Vérifiez la hiérarchie LevelDressing.");
    }

    static void AutoWirePrefabs(LevelDressingSpawner spawner)
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

    static void Assign(SerializedObject so, string prop, string assetPath)
    {
        var p = so.FindProperty(prop);
        if (p == null) return;
        p.objectReferenceValue = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
    }
}
#endif