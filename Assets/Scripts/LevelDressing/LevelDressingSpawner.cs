using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Set dressing technique — instancie tous les prefabs aux coordonnées blockout.
/// Menu Editor : Bee Climate Jam → Build Level Dressing (0-500m)
/// </summary>
public class LevelDressingSpawner : MonoBehaviour
{
    [Header("Prefabs projet")]
    [SerializeField] private GameObject beePrefab;
    [SerializeField] private GameObject flowerPrefab;
    [SerializeField] private GameObject branchPrefab;
    [SerializeField] private GameObject hornetPrefab;
    [SerializeField] private GameObject pesticidePrefab;
    [SerializeField] private GameObject nectarPrefab;

    [Header("Options")]
    [SerializeField] private bool spawnOnAwake = true;
    [SerializeField] private bool clearBeforeSpawn = true;
    [SerializeField] private Transform levelRoot;

    private readonly List<GameObject> _spawned = new();

    private void Awake()
    {
        if (spawnOnAwake)
            BuildLevel();
    }

    public void BuildLevel()
    {
        if (clearBeforeSpawn)
            ClearSpawned();

        var root = levelRoot != null ? levelRoot : transform;

        foreach (var entry in LevelDressingCatalog.All)
        {
            var pos = entry.WorldPosition;
            GameObject instance = null;

            switch (entry.Type)
            {
                case LevelDressingCatalog.Kind.Spawn:
                    if (FindAnyObjectByType<BeeController>() == null && beePrefab != null)
                        instance = Instantiate(beePrefab, pos, Quaternion.identity, root);
                    else
                        instance = CreateMarker(entry.Id, pos, root, "SpawnPoint");
                    break;

                case LevelDressingCatalog.Kind.Flower:
                case LevelDressingCatalog.Kind.FlowerWilted:
                    instance = SpawnPrefab(flowerPrefab, entry, root);
                    break;

                case LevelDressingCatalog.Kind.DecorPlant:
                    instance = SpawnPrefab(flowerPrefab, entry, root, 0.55f);
                    break;

                case LevelDressingCatalog.Kind.BranchObstacle:
                case LevelDressingCatalog.Kind.TrunkSmall:
                case LevelDressingCatalog.Kind.TrunkGiant:
                case LevelDressingCatalog.Kind.Goulet:
                case LevelDressingCatalog.Kind.FunnelArc:
                case LevelDressingCatalog.Kind.RushWall:
                case LevelDressingCatalog.Kind.Shelter:
                    instance = SpawnPrefab(branchPrefab, entry, root);
                    break;

                case LevelDressingCatalog.Kind.Nectar:
                    instance = SpawnPrefab(nectarPrefab, entry, root);
                    break;

                case LevelDressingCatalog.Kind.HornetPatrol:
                case LevelDressingCatalog.Kind.HornetStatic:
                    instance = SpawnPrefab(hornetPrefab, entry, root);
                    if (instance != null && entry.Type == LevelDressingCatalog.Kind.HornetPatrol)
                        instance.AddComponent<HornetPatrol>();
                    break;

                case LevelDressingCatalog.Kind.HornetChaseTrigger:
                    instance = CreateTrigger(entry.Id, pos, entry.Scale, root);
                    if (instance != null)
                    {
                        var chase = instance.AddComponent<HornetChaseTrigger>();
                        chase.Configure(hornetPrefab);
                    }
                    break;

                case LevelDressingCatalog.Kind.PesticideCloud:
                case LevelDressingCatalog.Kind.PesticideFloor:
                    instance = SpawnPrefab(pesticidePrefab, entry, root);
                    break;

                case LevelDressingCatalog.Kind.VictoryTrigger:
                    instance = CreateTrigger(entry.Id, pos, entry.Scale, root);
                    if (instance != null)
                        instance.AddComponent<VictoryZone>();
                    break;
            }

            if (instance != null)
            {
                instance.name = entry.Id;
                _spawned.Add(instance);
            }
        }

        Debug.Log($"[LevelDressing] {_spawned.Count} objets placés (blockout 0–500 m).");
    }

    public void ClearSpawned()
    {
        foreach (var go in _spawned)
        {
            if (go == null) continue;
#if UNITY_EDITOR
            if (!Application.isPlaying)
                DestroyImmediate(go);
            else
#endif
                Destroy(go);
        }
        _spawned.Clear();

        if (levelRoot == null)
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
#if UNITY_EDITOR
                if (!Application.isPlaying)
                    DestroyImmediate(transform.GetChild(i).gameObject);
                else
#endif
                    Destroy(transform.GetChild(i).gameObject);
            }
        }
    }

    static GameObject SpawnPrefab(GameObject prefab, LevelDressingCatalog.Entry entry, Transform root, float scaleMul = 1f)
    {
        if (prefab == null)
            return CreateMarker(entry.Id, entry.WorldPosition, root, entry.Type.ToString());

        var rot = Quaternion.Euler(0, 0, entry.RotZ);
        var go = Instantiate(prefab, entry.WorldPosition, rot, root);
        go.transform.localScale = entry.Scale * scaleMul;
        return go;
    }

    static GameObject CreateTrigger(string id, Vector3 pos, Vector3 scale, Transform root)
    {
        var go = new GameObject(id);
        go.transform.SetParent(root, false);
        go.transform.position = pos;
        go.transform.localScale = scale;
        var col = go.AddComponent<BoxCollider2D>();
        col.isTrigger = true;
        col.size = Vector2.one;
        return go;
    }

    static GameObject CreateMarker(string id, Vector3 pos, Transform root, string label)
    {
        var go = new GameObject($"{id}_{label}");
        go.transform.SetParent(root, false);
        go.transform.position = pos;
        return go;
    }
}