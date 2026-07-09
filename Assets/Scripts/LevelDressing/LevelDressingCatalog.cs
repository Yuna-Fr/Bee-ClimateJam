using System.Collections.Generic;
using UnityEngine;

/// <summary>Catalogue spatial enrichi — routes multiples · pesticides aléatoires.</summary>
public static class LevelDressingCatalog
{
    public enum Kind
    {
        Spawn,
        Flower,
        FlowerWilted,
        DecorPlant,
        BranchObstacle,
        TrunkSmall,
        TrunkGiant,
        Goulet,
        FunnelArc,
        RushWall,
        Nectar,
        PesticideCloud,
        PesticideFloor,
        HornetPatrol,
        HornetStatic,
        HornetChaseTrigger,
        Shelter,
        VictoryTrigger,
    }

    public readonly struct Entry
    {
        public readonly string Id; public readonly Kind Type;
        public readonly float Gx, Gy, Gz; public readonly Vector3 Scale; public readonly float RotZ;
        public Entry(string id, Kind type, float gx, float gy, float gz,
            float sx = 1f, float sy = 1f, float sz = 1f, float rotZ = 0f)
        { Id = id; Type = type; Gx = gx; Gy = gy; Gz = gz;
          Scale = new Vector3(sx, sy, sz); RotZ = rotZ; }
        public Vector3 WorldPosition => new(Gz, Gy, 0f);
    }
    public static IReadOnlyList<Entry> All => Entries;
    static readonly List<Entry> Entries = Build();
    static List<Entry> Build()
    {
        var e = new List<Entry>();
        e.Add(new Entry("Spawn_001", Kind.Spawn, 0f, 0f, 0f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("Bush_001", Kind.DecorPlant, 0f, -3.527f, 8.291f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("FlowerWilted_001", Kind.FlowerWilted, 0f, -3.023f, 10.87f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("Bush_002", Kind.DecorPlant, 0f, 1.141f, 13.29f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("ScatterNectar_001", Kind.Nectar, 0f, 4.449f, 13.71f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("ScatterDecor_001", Kind.DecorPlant, 0f, 1.563f, 15.11f, 0.6f, 0.6f, 1f, 0f));
        e.Add(new Entry("Bush_003", Kind.DecorPlant, 0f, -3.393f, 21.92f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("Nectar_001", Kind.Nectar, 0f, 2.986f, 27.52f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("Flower_001", Kind.Flower, 0f, 3.731f, 28.12f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("FlowerWilted_002", Kind.FlowerWilted, 0f, -3.175f, 28.4f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("Bush_004", Kind.DecorPlant, 0f, -4.765f, 30.52f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("ScatterDecor_002", Kind.DecorPlant, 0f, -2.343f, 32.96f, 0.6f, 0.6f, 1f, 0f));
        e.Add(new Entry("Bush_005", Kind.DecorPlant, 0f, 3.911f, 35.65f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("FlowerWilted_003", Kind.FlowerWilted, 0f, 0.8698f, 40.18f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("Nectar_Explore_021", Kind.Nectar, 0f, -3.483f, 45.26f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("ExploreFlower_BIF-A_01", Kind.Flower, 0f, 4.837f, 45.88f, 0.85f, 0.85f, 1f, 0f));
        e.Add(new Entry("Branch_001", Kind.BranchObstacle, 0f, -3.553f, 50f, 1f, 2.2f, 1f, 0f));
        e.Add(new Entry("ScatterDecor_003", Kind.DecorPlant, 0f, 2.857f, 51.4f, 0.6f, 0.6f, 1f, 0f));
        e.Add(new Entry("FlowerExplore_001", Kind.Flower, 0f, 4.09f, 52f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("SmallPlant_001", Kind.DecorPlant, 0f, -4.866f, 52.61f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("ExploreFlower_BIF-A_02", Kind.Flower, 0f, 4.14f, 54.3f, 0.85f, 0.85f, 1f, 0f));
        e.Add(new Entry("Nectar_Explore_022", Kind.Nectar, 0f, 4.833f, 56.2f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("TrunkSmall_001", Kind.TrunkSmall, 0f, -2.639f, 58f, 1.3f, 1.3f, 1f, 0f));
        e.Add(new Entry("FlowerWilted_004", Kind.FlowerWilted, 0f, -3.533f, 62f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("FlowerExplore_002", Kind.Flower, 0f, 4.457f, 65.68f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("Branch_002", Kind.BranchObstacle, 0f, 2.797f, 66f, 1.3f, 1.3f, 1f, 15f));
        e.Add(new Entry("ExploreFlower_BIF-A_03", Kind.Flower, 0f, -4.16f, 67.54f, 0.85f, 0.85f, 1f, 0f));
        e.Add(new Entry("ScatterNectar_004", Kind.Nectar, 0f, -4.87f, 67.7f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("Nectar_Explore_023", Kind.Nectar, 0f, 4.168f, 69.07f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("ScatterDecor_004", Kind.DecorPlant, 0f, -4.871f, 69.24f, 0.6f, 0.6f, 1f, 0f));
        e.Add(new Entry("Nectar_002", Kind.Nectar, 0f, 3.009f, 70.63f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("FlowerWilted_005", Kind.FlowerWilted, 0f, -4.237f, 72.69f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("Flower_002", Kind.Flower, 0f, -3.866f, 73.15f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("Branch_003", Kind.BranchObstacle, 0f, 4.568f, 74f, 1f, 2.2f, 1f, -30f));
        e.Add(new Entry("FlowerWilted_006", Kind.FlowerWilted, 0f, -2.997f, 75.52f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("FlowerExplore_003", Kind.Flower, 0f, -4.402f, 78.4f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("Pesticide_Rand_001", Kind.PesticideCloud, 0f, -2.359f, 79f, 1.201f, 1.441f, 1f, 8.6f));
        e.Add(new Entry("SmallPlant_002", Kind.DecorPlant, 0f, -1.873f, 79.47f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("FunnelArc_001", Kind.FunnelArc, 0f, -2.774f, 80f, 1.1f, 2f, 1f, 28f));
        e.Add(new Entry("FunnelArc_002", Kind.FunnelArc, 0f, 2.726f, 80f, 1.1f, 2f, 1f, -28f));
        e.Add(new Entry("Pesticide_Rand_002", Kind.PesticideCloud, 0f, -3.976f, 80.79f, 1.148f, 1.26f, 1f, 34.2f));
        e.Add(new Entry("FlowerWilted_007", Kind.FlowerWilted, 0f, -3.499f, 80.8f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("Pesticide_Rand_003", Kind.PesticideCloud, 0f, -3.79f, 80.82f, 1.177f, 1.184f, 1f, -29.1f));
        e.Add(new Entry("TrunkSmall_002", Kind.TrunkSmall, 0f, 0.6284f, 82f, 1.3f, 1.3f, 1f, 0f));
        e.Add(new Entry("ExploreFlower_BIF-A_04", Kind.Flower, 0f, 3.522f, 82.21f, 0.85f, 0.85f, 1f, 0f));
        e.Add(new Entry("Nectar_Explore_024", Kind.Nectar, 0f, 3.194f, 82.66f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("Pesticide_Rand_007", Kind.PesticideCloud, 0f, 3.511f, 85.84f, 1.298f, 1.417f, 1f, -13.2f));
        e.Add(new Entry("FunnelArc_003", Kind.FunnelArc, 0f, -2.776f, 86f, 1.1f, 2f, 1f, 28f));
        e.Add(new Entry("FunnelArc_004", Kind.FunnelArc, 0f, 2.724f, 86f, 1.1f, 2f, 1f, -28f));
        e.Add(new Entry("Pesticide_Rand_005", Kind.PesticideCloud, 0f, 1.53f, 87.21f, 1.244f, 1.355f, 1f, 10.5f));
        e.Add(new Entry("Pesticide_Rand_004", Kind.PesticideCloud, 0f, -3.709f, 87.38f, 1.41f, 1.548f, 1f, -39f));
        e.Add(new Entry("Pesticide_Rand_006", Kind.PesticideCloud, 0f, -3.856f, 87.82f, 1.033f, 1.192f, 1f, 19f));
        e.Add(new Entry("Branch_004", Kind.BranchObstacle, 0f, 1.718f, 88f, 1.3f, 1.3f, 1f, -12f));
        e.Add(new Entry("ScatterDecor_005", Kind.DecorPlant, 0f, 2.677f, 88.94f, 0.6f, 0.6f, 1f, 0f));
        e.Add(new Entry("FlowerExplore_004", Kind.Flower, 0f, -1.3f, 89.46f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("FlowerWilted_008", Kind.FlowerWilted, 0f, -3.095f, 89.5f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("Pesticide_Rand_009", Kind.PesticideCloud, 0f, -4.321f, 90.95f, 1.104f, 1.132f, 1f, -31.5f));
        e.Add(new Entry("Pesticide_Rand_008", Kind.PesticideCloud, 0f, -4.259f, 91.87f, 1.424f, 1.722f, 1f, 23.7f));
        e.Add(new Entry("FunnelArc_005", Kind.FunnelArc, 0f, -2.778f, 94f, 1.1f, 2f, 1f, 28f));
        e.Add(new Entry("FunnelArc_006", Kind.FunnelArc, 0f, 2.722f, 94f, 1.1f, 2f, 1f, -28f));
        e.Add(new Entry("ExploreFlower_BIF-A_05", Kind.Flower, 0f, -1.636f, 95.54f, 0.85f, 0.85f, 1f, 0f));
        e.Add(new Entry("Nectar_Explore_025", Kind.Nectar, 0f, 2.449f, 95.9f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("Pesticide_Rand_010", Kind.PesticideCloud, 0f, -3.828f, 99.15f, 1.582f, 1.692f, 1f, -20.3f));
        e.Add(new Entry("FlowerWilted_009", Kind.FlowerWilted, 0f, 2.465f, 99.17f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("FunnelArc_007", Kind.FunnelArc, 0f, -2.78f, 100f, 1.1f, 2f, 1f, 28f));
        e.Add(new Entry("FunnelArc_008", Kind.FunnelArc, 0f, 2.72f, 100f, 1.1f, 2f, 1f, -28f));
        e.Add(new Entry("Flower_003", Kind.Flower, 0f, 2.372f, 100.5f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("SmallPlant_003", Kind.DecorPlant, 0f, -1.088f, 100.5f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("ScatterDecor_006", Kind.DecorPlant, 0f, -4.877f, 104.2f, 0.6f, 0.6f, 1f, 0f));
        e.Add(new Entry("MazeObstacle_01", Kind.BranchObstacle, 0f, 3.576f, 108f, 2.2f, 3.5f, 1f, 0f));
        e.Add(new Entry("FlowerWilted_010", Kind.FlowerWilted, 0f, 1.924f, 111f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("MazeObstacle_02", Kind.BranchObstacle, 0f, -3.417f, 118.2f, 2.5f, 3f, 1f, 90f));
        e.Add(new Entry("FlowerWilted_011", Kind.FlowerWilted, 0f, 3.674f, 119.5f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("ScatterNectar_007", Kind.Nectar, 0f, 3.218f, 122.6f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("ScatterDecor_007", Kind.DecorPlant, 0f, 4.466f, 123.9f, 0.6f, 0.6f, 1f, 0f));
        e.Add(new Entry("FunnelArc_009", Kind.FunnelArc, 0f, -2.363f, 124f, 1.1f, 2f, 1f, 28f));
        e.Add(new Entry("FunnelArc_010", Kind.FunnelArc, 0f, 2.337f, 124f, 1.1f, 2f, 1f, -28f));
        e.Add(new Entry("Pesticide_Rand_012", Kind.PesticideCloud, 0f, 1.964f, 124f, 0.937f, 1.071f, 1f, -33.7f));
        e.Add(new Entry("Pesticide_Rand_015", Kind.PesticideCloud, 0f, 1.579f, 124f, 1.24f, 1.155f, 1f, 31.1f));
        e.Add(new Entry("Pesticide_Rand_013", Kind.PesticideCloud, 0f, 2.96f, 124.8f, 0.883f, 0.816f, 1f, 19.9f));
        e.Add(new Entry("Pesticide_Rand_014", Kind.PesticideCloud, 0f, -2.884f, 125.3f, 1.126f, 1.159f, 1f, -12.4f));
        e.Add(new Entry("Pesticide_Rand_011", Kind.PesticideCloud, 0f, 2.838f, 125.7f, 0.949f, 1.081f, 1f, 19.1f));
        e.Add(new Entry("Pesticide_Rand_016", Kind.PesticideCloud, 0f, -3.475f, 128.5f, 1.558f, 1.339f, 1f, 38.2f));
        e.Add(new Entry("SmallPlant_004", Kind.DecorPlant, 0f, -2.558f, 130.6f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("Pesticide_Rand_021", Kind.PesticideCloud, 0f, 2.986f, 130.7f, 1.01f, 0.858f, 1f, -41.7f));
        e.Add(new Entry("Pesticide_Rand_018", Kind.PesticideCloud, 0f, -4.215f, 131.2f, 1.394f, 1.311f, 1f, -41.3f));
        e.Add(new Entry("Pesticide_Rand_019", Kind.PesticideCloud, 0f, -4.235f, 131.3f, 1.432f, 1.528f, 1f, -36.3f));
        e.Add(new Entry("Pesticide_Rand_017", Kind.PesticideCloud, 0f, 2.193f, 131.5f, 1.305f, 1.205f, 1f, 2.4f));
        e.Add(new Entry("FunnelArc_011", Kind.FunnelArc, 0f, -2.358f, 132f, 1.1f, 2f, 1f, 28f));
        e.Add(new Entry("FunnelArc_012", Kind.FunnelArc, 0f, 2.342f, 132f, 1.1f, 2f, 1f, -28f));
        e.Add(new Entry("Pesticide_Rand_020", Kind.PesticideCloud, 0f, 3.642f, 132.3f, 1.132f, 1.195f, 1f, -35f));
        e.Add(new Entry("MazeObstacle_03", Kind.BranchObstacle, 0f, 4.092f, 132.5f, 2f, 2.8f, 1f, 0f));
        e.Add(new Entry("SmallPlant_005", Kind.DecorPlant, 0f, 4.522f, 134.4f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("Pesticide_Rand_025", Kind.PesticideCloud, 0f, 2.46f, 135.2f, 0.866f, 0.815f, 1f, -17.4f));
        e.Add(new Entry("Pesticide_Rand_024", Kind.PesticideCloud, 0f, -2.471f, 135.9f, 1.514f, 1.616f, 1f, 17.2f));
        e.Add(new Entry("Pesticide_Rand_026", Kind.PesticideCloud, 0f, -4.033f, 136.1f, 1.548f, 1.418f, 1f, -21.5f));
        e.Add(new Entry("Pesticide_Rand_022", Kind.PesticideCloud, 0f, -1.98f, 136.9f, 1.315f, 1.207f, 1f, 5.2f));
        e.Add(new Entry("Pesticide_Rand_027", Kind.PesticideCloud, 0f, -1.822f, 137.7f, 1.533f, 1.909f, 1f, 33.9f));
        e.Add(new Entry("Pesticide_Rand_023", Kind.PesticideCloud, 0f, 2.181f, 138f, 1.177f, 1.264f, 1f, 4.3f));
        e.Add(new Entry("Pesticide_Rand_029", Kind.PesticideCloud, 0f, -3.982f, 138f, 0.932f, 0.801f, 1f, 2.8f));
        e.Add(new Entry("Pesticide_Rand_034", Kind.PesticideCloud, 0f, 4.307f, 138.5f, 1.551f, 1.453f, 1f, -32.6f));
        e.Add(new Entry("Pesticide_Rand_032", Kind.PesticideCloud, 0f, -2.099f, 138.6f, 1.195f, 1.217f, 1f, 35.8f));
        e.Add(new Entry("Pesticide_Rand_030", Kind.PesticideCloud, 0f, 1.547f, 138.7f, 1.144f, 1.402f, 1f, 23.5f));
        e.Add(new Entry("Pesticide_Rand_028", Kind.PesticideCloud, 0f, -2.14f, 139.6f, 1.204f, 1.349f, 1f, 8.2f));
        e.Add(new Entry("Pesticide_Rand_033", Kind.PesticideCloud, 0f, -3.222f, 140.6f, 1.085f, 0.919f, 1f, -19.6f));
        e.Add(new Entry("Pesticide_Rand_031", Kind.PesticideCloud, 0f, -2.946f, 140.6f, 1.483f, 1.467f, 1f, -16f));
        e.Add(new Entry("FunnelArc_013", Kind.FunnelArc, 0f, -2.351f, 142f, 1.1f, 2f, 1f, 28f));
        e.Add(new Entry("FunnelArc_014", Kind.FunnelArc, 0f, 2.349f, 142f, 1.1f, 2f, 1f, -28f));
        e.Add(new Entry("ScatterDecor_008", Kind.DecorPlant, 0f, -3.691f, 142.3f, 0.6f, 0.6f, 1f, 0f));
        e.Add(new Entry("SmallPlant_006", Kind.DecorPlant, 0f, -4.85f, 142.3f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("Pesticide_Rand_035", Kind.PesticideCloud, 0f, -2.21f, 143.2f, 1.532f, 1.356f, 1f, -41.5f));
        e.Add(new Entry("Pesticide_Rand_038", Kind.PesticideCloud, 0f, 2.382f, 146.3f, 1.147f, 1.252f, 1f, -39.5f));
        e.Add(new Entry("Pesticide_Rand_037", Kind.PesticideCloud, 0f, -1.917f, 147f, 1.551f, 1.41f, 1f, 7.1f));
        e.Add(new Entry("MazeObstacle_04", Kind.BranchObstacle, 0f, -3.796f, 148f, 3f, 2.2f, 1f, 45f));
        e.Add(new Entry("SmallPlant_007", Kind.DecorPlant, 0f, -4.846f, 148.6f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("Pesticide_Rand_039", Kind.PesticideCloud, 0f, -1.673f, 148.7f, 0.819f, 0.862f, 1f, 3.3f));
        e.Add(new Entry("Pesticide_Rand_036", Kind.PesticideCloud, 0f, 2.802f, 148.7f, 0.881f, 0.88f, 1f, -15.8f));
        e.Add(new Entry("FunnelArc_015", Kind.FunnelArc, 0f, -2.345f, 150f, 1.1f, 2f, 1f, 28f));
        e.Add(new Entry("FunnelArc_016", Kind.FunnelArc, 0f, 2.355f, 150f, 1.1f, 2f, 1f, -28f));
        e.Add(new Entry("Flower_004", Kind.Flower, 0f, 3.765f, 151.8f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("FlowerWilted_012", Kind.FlowerWilted, 0f, 4.856f, 152.1f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("SmallPlant_008", Kind.DecorPlant, 0f, 3.967f, 154.1f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("ExploreFlower_BIF-B_01", Kind.Flower, 0f, 4.585f, 154.5f, 0.85f, 0.85f, 1f, 0f));
        e.Add(new Entry("Nectar_Explore_026", Kind.Nectar, 0f, -3.706f, 155.4f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("FlowerWilted_013", Kind.FlowerWilted, 0f, 4.859f, 155.9f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("ScatterDecor_009", Kind.DecorPlant, 0f, 4.234f, 159.8f, 0.6f, 0.6f, 1f, 0f));
        e.Add(new Entry("TrunkSmall_003", Kind.TrunkSmall, 0f, -3.037f, 162f, 1.3f, 1.3f, 1f, 0f));
        e.Add(new Entry("MazeObstacle_05", Kind.BranchObstacle, 0f, 3.213f, 162.2f, 2f, 3.2f, 1f, 0f));
        e.Add(new Entry("FlowerExplore_005", Kind.Flower, 0f, 3.706f, 164.9f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("Nectar_Explore_027", Kind.Nectar, 0f, -2.624f, 168.4f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("ExploreFlower_BIF-B_02", Kind.Flower, 0f, 3.779f, 168.4f, 0.85f, 0.85f, 1f, 0f));
        e.Add(new Entry("Branch_005", Kind.BranchObstacle, 0f, 1.356f, 172f, 1f, 2.2f, 1f, 25f));
        e.Add(new Entry("SmallPlant_009", Kind.DecorPlant, 0f, -4.828f, 173f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("Nectar_003", Kind.Nectar, 0f, -1.404f, 174.3f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("FlowerWilted_014", Kind.FlowerWilted, 0f, -2.623f, 175.2f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("Flower_005", Kind.Flower, 0f, -2.604f, 175.7f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("ScatterNectar_010", Kind.Nectar, 0f, -4.145f, 176f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("MazeObstacle_06", Kind.BranchObstacle, 0f, -3.475f, 178.5f, 2.5f, 3.5f, 1f, -30f));
        e.Add(new Entry("FlowerWilted_015", Kind.FlowerWilted, 0f, -3.459f, 179f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("ScatterDecor_010", Kind.DecorPlant, 0f, -1.953f, 179.1f, 0.6f, 0.6f, 1f, 0f));
        e.Add(new Entry("FlowerExplore_006", Kind.Flower, 0f, -2.708f, 179.6f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("ExploreFlower_BIF-B_03", Kind.Flower, 0f, 2.05f, 181.2f, 0.85f, 0.85f, 1f, 0f));
        e.Add(new Entry("Branch_006", Kind.BranchObstacle, 0f, -0.996f, 182f, 1.3f, 1.3f, 1f, -12f));
        e.Add(new Entry("Nectar_Explore_028", Kind.Nectar, 0f, 1.891f, 182.7f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("TrunkSmall_004", Kind.TrunkSmall, 0f, -3.543f, 192f, 1.3f, 1.3f, 1f, 0f));
        e.Add(new Entry("ScatterDecor_011", Kind.DecorPlant, 0f, 4.886f, 195.5f, 0.6f, 0.6f, 1f, 0f));
        e.Add(new Entry("ExploreFlower_BIF-B_04", Kind.Flower, 0f, 3.858f, 195.7f, 0.85f, 0.85f, 1f, 0f));
        e.Add(new Entry("Nectar_Explore_029", Kind.Nectar, 0f, -3.179f, 196.7f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("Branch_007", Kind.BranchObstacle, 0f, 1.432f, 198f, 1f, 2.2f, 1f, 30f));
        e.Add(new Entry("FlowerExplore_007", Kind.Flower, 0f, -2.608f, 198.4f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("SmallPlant_010", Kind.DecorPlant, 0f, 4.885f, 203.2f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("ExploreFlower_BIF-B_05", Kind.Flower, 0f, 1.818f, 204.9f, 0.85f, 0.85f, 1f, 0f));
        e.Add(new Entry("Hornet_001", Kind.HornetPatrol, 0f, 4.487f, 205f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("MazeObstacle_07", Kind.BranchObstacle, 0f, 3.833f, 205f, 2f, 2.8f, 1f, 0f));
        e.Add(new Entry("Nectar_Explore_030", Kind.Nectar, 0f, -3.193f, 205.8f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("BypassBranch_01", Kind.BranchObstacle, 0f, 0.2691f, 206f, 1.2f, 1.8f, 1f, -20f));
        e.Add(new Entry("ScatterDecor_012", Kind.DecorPlant, 0f, -4.83f, 213.7f, 0.6f, 0.6f, 1f, 0f));
        e.Add(new Entry("FlowerWilted_016", Kind.FlowerWilted, 0f, 4.448f, 214.3f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("BypassBranch_02", Kind.BranchObstacle, 0f, 1.307f, 214.3f, 1.2f, 1.8f, 1f, 15f));
        e.Add(new Entry("Nectar_Bypass_02", Kind.Nectar, 0f, 3.464f, 215.1f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("Hornet_002", Kind.HornetStatic, 0f, -1.653f, 218f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("BypassBranch_03", Kind.BranchObstacle, 0f, 1.995f, 222.6f, 1.2f, 1.8f, 1f, -20f));
        e.Add(new Entry("TrunkGiant_001", Kind.TrunkGiant, 0f, -0.0016f, 228f, 4.5f, 4.5f, 1f, 0f));
        e.Add(new Entry("Nectar_Bypass_04", Kind.Nectar, 0f, 3.032f, 228.1f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("BypassBranch_04", Kind.BranchObstacle, 0f, 2.224f, 228.9f, 1.2f, 1.8f, 1f, 15f));
        e.Add(new Entry("ScatterNectar_013", Kind.Nectar, 0f, 2.844f, 230.8f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("ScatterDecor_013", Kind.DecorPlant, 0f, 4.348f, 232.1f, 0.6f, 0.6f, 1f, 0f));
        e.Add(new Entry("BypassBranch_05", Kind.BranchObstacle, 0f, 1.736f, 237.2f, 1.2f, 1.8f, 1f, -20f));
        e.Add(new Entry("MazeObstacle_08", Kind.BranchObstacle, 0f, -3.116f, 238.2f, 2.2f, 3f, 1f, 0f));
        e.Add(new Entry("Nectar_Bypass_06", Kind.Nectar, 0f, -4.055f, 244.3f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("BypassBranch_06", Kind.BranchObstacle, 0f, 1.011f, 245.5f, 1.2f, 1.8f, 1f, 15f));
        e.Add(new Entry("SmallPlant_011", Kind.DecorPlant, 0f, -3.746f, 248f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("ScatterDecor_014", Kind.DecorPlant, 0f, -3.765f, 250.9f, 0.6f, 0.6f, 1f, 0f));
        e.Add(new Entry("BypassBranch_07", Kind.BranchObstacle, 0f, 0.3967f, 253.8f, 1.2f, 1.8f, 1f, -20f));
        e.Add(new Entry("MazeObstacle_09", Kind.BranchObstacle, 0f, 3.354f, 258.5f, 2f, 2.5f, 1f, 0f));
        e.Add(new Entry("Nectar_Bypass_08", Kind.Nectar, 0f, 2.816f, 261.7f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("BypassBranch_08", Kind.BranchObstacle, 0f, 1.548f, 264.1f, 1.2f, 1.8f, 1f, 15f));
        e.Add(new Entry("Goulet_001", Kind.Goulet, 0f, -1.761f, 268f, 3f, 0.4f, 1f, 18f));
        e.Add(new Entry("Goulet_002", Kind.Goulet, 0f, 1.639f, 268f, 3f, 0.4f, 1f, -18f));
        e.Add(new Entry("ScatterDecor_015", Kind.DecorPlant, 0f, 3.178f, 268.8f, 0.6f, 0.6f, 1f, 0f));
        e.Add(new Entry("FlowerWilted_017", Kind.FlowerWilted, 0f, 2.488f, 271.1f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("Goulet_003", Kind.Goulet, 0f, -1.77f, 274f, 3f, 0.4f, 1f, 18f));
        e.Add(new Entry("Goulet_004", Kind.Goulet, 0f, -1.52f, 274f, 0.4f, 3.5f, 1f, 0f));
        e.Add(new Entry("Goulet_005", Kind.Goulet, 0f, 1.38f, 274f, 0.4f, 3.5f, 1f, 0f));
        e.Add(new Entry("Goulet_006", Kind.Goulet, 0f, 1.63f, 274f, 3f, 0.4f, 1f, -18f));
        e.Add(new Entry("Goulet_007", Kind.Goulet, 0f, -1.75f, 283f, 3f, 0.4f, 1f, 18f));
        e.Add(new Entry("Goulet_008", Kind.Goulet, 0f, 1.65f, 283f, 3f, 0.4f, 1f, -18f));
        e.Add(new Entry("SmallPlant_012", Kind.DecorPlant, 0f, -1.361f, 284.2f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("ScatterNectar_016", Kind.Nectar, 0f, -3.671f, 285.3f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("ScatterDecor_016", Kind.DecorPlant, 0f, -4.895f, 285.6f, 0.6f, 0.6f, 1f, 0f));
        e.Add(new Entry("Goulet_009", Kind.Goulet, 0f, -1.721f, 296f, 3f, 0.4f, 1f, 18f));
        e.Add(new Entry("Goulet_010", Kind.Goulet, 0f, 1.679f, 296f, 3f, 0.4f, 1f, -18f));
        e.Add(new Entry("ScatterDecor_017", Kind.DecorPlant, 0f, 3.667f, 302.8f, 0.6f, 0.6f, 1f, 0f));
        e.Add(new Entry("Nectar_004", Kind.Nectar, 0f, 4.866f, 311.6f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("MeadowFlower_FL-01", Kind.Flower, 0f, -4.284f, 311.6f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("MazeObstacle_10", Kind.BranchObstacle, 0f, -3.585f, 312f, 2.5f, 3f, 1f, 15f));
        e.Add(new Entry("FlowerWilted_018", Kind.FlowerWilted, 0f, 4.865f, 312.6f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("Flower_006", Kind.Flower, 0f, 4.748f, 312.7f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("MeadowNectar_FL-01", Kind.Nectar, 0f, 2.204f, 312.7f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("SmallPlant_013", Kind.DecorPlant, 0f, 2.618f, 318.6f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("Flower_007", Kind.Flower, 0f, 4.65f, 321.1f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("MeadowFlower_FL-02", Kind.Flower, 0f, -3.37f, 321.6f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("FlowerWilted_019", Kind.FlowerWilted, 0f, 4.464f, 321.7f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("ScatterDecor_018", Kind.DecorPlant, 0f, -2.818f, 322.4f, 0.6f, 0.6f, 1f, 0f));
        e.Add(new Entry("MazeObstacle_11", Kind.BranchObstacle, 0f, 3.551f, 328.2f, 2f, 2.5f, 1f, 0f));
        e.Add(new Entry("Flower_008", Kind.Flower, 0f, 3.981f, 331.3f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("Nectar_005", Kind.Nectar, 0f, -4.79f, 331.6f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("MeadowNectar_FL-03", Kind.Nectar, 0f, 1.627f, 331.6f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("FlowerWilted_020", Kind.FlowerWilted, 0f, 1.891f, 332f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("MeadowFlower_FL-03", Kind.Flower, 0f, 4.91f, 332.5f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("SmallPlant_014", Kind.DecorPlant, 0f, 2.55f, 337.5f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("ScatterNectar_019", Kind.Nectar, 0f, 4.924f, 338.6f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("ScatterDecor_019", Kind.DecorPlant, 0f, 3.218f, 340.2f, 0.6f, 0.6f, 1f, 0f));
        e.Add(new Entry("FlowerWilted_021", Kind.FlowerWilted, 0f, 1.159f, 341.7f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("MeadowFlower_FL-04", Kind.Flower, 0f, -3.752f, 342.1f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("Flower_009", Kind.Flower, 0f, 1.401f, 342.9f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("ExploreFlower_BIF-C_01", Kind.Flower, 0f, 3.468f, 35f, 0.85f, 0.85f, 1f, 0f));
        e.Add(new Entry("Nectar_Explore_031", Kind.Nectar, 0f, 3.801f, 350.4f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("FlowerWilted_022", Kind.FlowerWilted, 0f, 2.697f, 351.3f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("Flower_010", Kind.Flower, 0f, 2.057f, 351.5f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("MeadowNectar_FL-05", Kind.Nectar, 0f, 2.454f, 351.9f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("MeadowFlower_FL-05", Kind.Flower, 0f, -4.321f, 352f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("Nectar_006", Kind.Nectar, 0f, -3.685f, 352.3f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("MazeObstacle_12", Kind.BranchObstacle, 0f, -3.096f, 355.5f, 2.5f, 3f, 1f, -20f));
        e.Add(new Entry("FlowerExplore_008", Kind.Flower, 0f, 2.666f, 358.4f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("SmallPlant_015", Kind.DecorPlant, 0f, 2.23f, 358.7f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("ScatterDecor_020", Kind.DecorPlant, 0f, -4.743f, 359.1f, 0.6f, 0.6f, 1f, 0f));
        e.Add(new Entry("Flower_011", Kind.Flower, 0f, 3.969f, 361.2f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("Nectar_Explore_032", Kind.Nectar, 0f, 3.196f, 361.7f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("FlowerWilted_023", Kind.FlowerWilted, 0f, 2.387f, 361.8f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("MeadowFlower_FL-06", Kind.Flower, 0f, 3.761f, 361.8f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("ExploreFlower_BIF-C_02", Kind.Flower, 0f, 4.146f, 362.3f, 0.85f, 0.85f, 1f, 0f));
        e.Add(new Entry("Nectar_007", Kind.Nectar, 0f, 4.491f, 371.6f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("FlowerWilted_024", Kind.FlowerWilted, 0f, 4.22f, 371.7f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("MeadowFlower_FL-07", Kind.Flower, 0f, -3.137f, 372.2f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("MeadowNectar_FL-07", Kind.Nectar, 0f, 3.07f, 372.4f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("Flower_012", Kind.Flower, 0f, 3.497f, 372.8f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("ScatterDecor_021", Kind.DecorPlant, 0f, 2.506f, 375.3f, 0.6f, 0.6f, 1f, 0f));
        e.Add(new Entry("FlowerExplore_009", Kind.Flower, 0f, 4.97f, 375.5f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("ExploreFlower_BIF-C_03", Kind.Flower, 0f, -3.052f, 375.5f, 0.85f, 0.85f, 1f, 0f));
        e.Add(new Entry("Nectar_Explore_033", Kind.Nectar, 0f, -3.97f, 375.9f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("SmallPlant_016", Kind.DecorPlant, 0f, 4.972f, 377.2f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("MeadowFlower_FL-08", Kind.Flower, 0f, 2.176f, 382.4f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("Flower_013", Kind.Flower, 0f, 4.976f, 382.7f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("FlowerWilted_025", Kind.FlowerWilted, 0f, 4.976f, 382.7f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("Flower_014", Kind.Flower, 0f, 1.988f, 387.4f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("ExploreFlower_BIF-C_04", Kind.Flower, 0f, 3.485f, 387.5f, 0.85f, 0.85f, 1f, 0f));
        e.Add(new Entry("FlowerWilted_026", Kind.FlowerWilted, 0f, -2.706f, 387.5f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("MazeObstacle_13", Kind.BranchObstacle, 0f, 3.03f, 388f, 2f, 2.5f, 1f, 0f));
        e.Add(new Entry("Nectar_Explore_034", Kind.Nectar, 0f, 1.508f, 388.5f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("MeadowFlower_FL-09", Kind.Flower, 0f, 3.076f, 389.1f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("FlowerExplore_010", Kind.Flower, 0f, -2.342f, 391.4f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("ScatterNectar_022", Kind.Nectar, 0f, -4.716f, 392.5f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("ScatterDecor_022", Kind.DecorPlant, 0f, -2.914f, 394.1f, 0.6f, 0.6f, 1f, 0f));
        e.Add(new Entry("SmallPlant_017", Kind.DecorPlant, 0f, 2.712f, 394.6f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("ExploreFlower_BIF-C_05", Kind.Flower, 0f, 1.865f, 399.5f, 0.85f, 0.85f, 1f, 0f));
        e.Add(new Entry("CouloirV_001", Kind.RushWall, 0f, -4.86f, 400f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("CouloirV_002", Kind.RushWall, 0f, 5.14f, 400f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("Nectar_Explore_035", Kind.Nectar, 0f, 4.991f, 400.9f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("Hornet_V_405", Kind.HornetStatic, 0f, 2.943f, 405f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("ScatterDecor_023", Kind.DecorPlant, 0f, 2.812f, 411.2f, 0.6f, 0.6f, 1f, 0f));
        e.Add(new Entry("MazeObstacle_14", Kind.BranchObstacle, 0f, -2.449f, 418.2f, 2f, 2.2f, 1f, 0f));
        e.Add(new Entry("SmallPlant_018", Kind.DecorPlant, 0f, 4.805f, 419.1f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("Hornet_V_420", Kind.HornetStatic, 0f, -2.248f, 420f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("CouloirV_003", Kind.RushWall, 0f, -4.42f, 425f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("CouloirV_004", Kind.RushWall, 0f, 4.73f, 425f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("ScatterDecor_024", Kind.DecorPlant, 0f, -4.692f, 43f, 0.6f, 0.6f, 1f, 0f));
        e.Add(new Entry("Hornet_V_435", Kind.HornetStatic, 0f, 2.261f, 435f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("MazeObstacle_15", Kind.BranchObstacle, 0f, 2.363f, 438.5f, 2f, 2f, 1f, 10f));
        e.Add(new Entry("SmallPlant_019", Kind.DecorPlant, 0f, -4.488f, 445.6f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("ScatterNectar_025", Kind.Nectar, 0f, 4.533f, 446.6f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("ScatterDecor_025", Kind.DecorPlant, 0f, 1.953f, 448.6f, 0.6f, 0.6f, 1f, 0f));
        e.Add(new Entry("CouloirV_005", Kind.RushWall, 0f, -3.98f, 450f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("CouloirV_006", Kind.RushWall, 0f, 4.32f, 450f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("Hornet_V_452", Kind.HornetStatic, 0f, -1.729f, 452f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("MazeObstacle_16", Kind.BranchObstacle, 0f, -1.727f, 455f, 2f, 2f, 1f, -10f));
        e.Add(new Entry("Hornet_003", Kind.HornetChaseTrigger, 0f, -4.821f, 465f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("ScatterDecor_026", Kind.DecorPlant, 0f, -3.445f, 465.9f, 0.6f, 0.6f, 1f, 0f));
        e.Add(new Entry("Hornet_V_468", Kind.HornetStatic, 0f, 1.781f, 468f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("SmallPlant_020", Kind.DecorPlant, 0f, -2.406f, 468.8f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("CouloirV_007", Kind.RushWall, 0f, -3.54f, 475f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("CouloirV_008", Kind.RushWall, 0f, 3.91f, 475f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("ScatterDecor_027", Kind.DecorPlant, 0f, 3.663f, 484.2f, 0.6f, 0.6f, 1f, 0f));
        e.Add(new Entry("CouloirV_009", Kind.RushWall, 0f, -3.1f, 500f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("CouloirV_010", Kind.RushWall, 0f, 3.5f, 500f, 1f, 1f, 1f, 0f));
        e.Add(new Entry("Hive_001", Kind.Shelter, 0f, -0.5895f, 500f, 2.5f, 1.5f, 1f, 0f));
        e.Add(new Entry("Victory_Trigger", Kind.VictoryTrigger, 0f, -0.5895f, 500f, 3f, 4f, 1f, 0f));
        return e; // 287 instances
    }
}
