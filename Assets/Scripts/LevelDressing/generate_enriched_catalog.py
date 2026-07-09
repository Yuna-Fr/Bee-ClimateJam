#!/usr/bin/env python3
"""
Catalogue enrichi — placement spatial large, pesticides aléatoires, routes multiples.
"""
from __future__ import annotations

import hashlib
import importlib.util
import json
import math
import random
from pathlib import Path

HERE = Path(__file__).resolve().parent
ROOT = HERE.parents[3]
SCAN = ROOT / "Production" / "LEVEL_BLOCKOUT_SCAN_RECAL.json"
if not SCAN.is_file():
    SCAN = ROOT / "Production" / "LEVEL_BLOCKOUT_SCAN.json"
OUT = HERE / "LevelDressingCatalog.cs"

SKIP = {"RouteSAFE", "RouteMarker", "LimiteNord", "LimiteSud", "Checkpoint", "Sol", "FlowerGroup", "Plateforme", "RouteExplore", "Pesticide"}

KIND_ENUM = [
    "Spawn", "Flower", "FlowerWilted", "DecorPlant", "BranchObstacle", "TrunkSmall", "TrunkGiant",
    "Goulet", "FunnelArc", "RushWall", "Nectar", "PesticideCloud", "PesticideFloor",
    "HornetPatrol", "HornetStatic", "HornetChaseTrigger", "Shelter", "VictoryTrigger",
]

PATH_KF = [(0, 0), (100, -0.03), (200, 0.04), (274, -0.07), (350, 0.1), (400, 0.14), (500, 0.2)]
SAFE_HALF = 1.25
CORRIDOR_HALF = 4.85
SPREAD_KINDS = {"Flower", "FlowerWilted", "DecorPlant", "Nectar"}
CRITICAL_KINDS = {
    "Spawn", "BranchObstacle", "TrunkSmall", "TrunkGiant", "Goulet", "FunnelArc", "RushWall",
    "HornetPatrol", "HornetStatic", "HornetChaseTrigger", "Shelter", "VictoryTrigger", "PesticideCloud",
}


def load_bg():
    spec = importlib.util.spec_from_file_location("blockout_geometry", ROOT / "Production" / "blockout_geometry.py")
    mod = importlib.util.module_from_spec(spec)
    spec.loader.exec_module(mod)
    return mod


def path_y(m: float) -> float:
    if m <= PATH_KF[0][0]:
        return PATH_KF[0][1]
    for i in range(len(PATH_KF) - 1):
        m0, y0 = PATH_KF[i]
        m1, y1 = PATH_KF[i + 1]
        if m0 <= m <= m1:
            t = (m - m0) / (m1 - m0) if m1 != m0 else 0.0
            return y0 + t * (y1 - y0)
    return PATH_KF[-1][1]


def wy_px(bg, m: float, off_px: float) -> float:
    return round(bg.path_y(m) + off_px / bg.PX_PER_M, 4)


def wy_m(bg, m: float, off_m: float) -> float:
    return round(bg.path_y(m) + off_m, 4)


def fmt_f(v: float) -> str:
    if v == int(v):
        return f"{int(v)}f"
    return f"{v:.4g}".rstrip("0").rstrip(".") + "f"


def _hash_vals(uid: str) -> tuple[float, float, float]:
    h = hashlib.md5(uid.encode()).hexdigest()
    return (
        int(h[0:8], 16) / 0xFFFFFFFF,
        int(h[8:16], 16) / 0xFFFFFFFF,
        int(h[16:24], 16) / 0xFFFFFFFF,
    )


def spread_spatial(uid: str, kind: str, gy: float, gz: float) -> tuple[float, float]:
    """Écarte verticalement + léger décalage X pour casser les alignements."""
    if kind not in SPREAD_KINDS:
        return gy, gz
    py = path_y(gz)
    t1, t2, t3 = _hash_vals(uid)

    gz += (t1 - 0.5) * 1.8
    gz = max(0.0, min(500.0, gz))

    if abs(gy - py) < 2.0:
        sign = 1.0 if t2 > 0.5 else -1.0
        gy = py + sign * (2.4 + t3 * 2.2)

    gy += math.sin(gz * 0.065 + t2 * 6.28) * 1.1
    gy += math.cos(gz * 0.031) * 0.55 * (1 if t3 > 0.5 else -1)

    gy = max(py - CORRIDOR_HALF, min(py + CORRIDOR_HALF, gy))
    return round(gy, 4), round(gz, 4)


def _pest_y_on_side(
    rng: random.Random, xm: float, fn: float, fs: float, side: str,
) -> float:
    """Place une tuile sur un bord, avec profondeur et ondulation variables."""
    base = fn if side == "N" else fs
    depth = rng.uniform(0.15, 1.75)
    y = base - depth if side == "N" else base + depth
    py = path_y(xm)
    y += rng.uniform(-1.1, 1.1)
    y += math.sin(xm * 0.11 + rng.uniform(0, math.tau)) * 0.75
    y += math.cos(xm * 0.047) * 0.4 * (1 if side == "N" else -1)
    if abs(y - py) < SAFE_HALF:
        y = py + (CORRIDOR_HALF - rng.uniform(0.35, 1.35)) * (1 if side == "N" else -1)
    return round(max(py - CORRIDOR_HALF, min(py + CORRIDOR_HALF, y)), 4)


def random_pesticides(bg) -> list[tuple]:
    """Nuages pesticide organiques — zigzag, grappes, trous, pas de double rail."""
    rng = random.Random(20260709)
    zones = [
        ("PestRand", "Z03_Pest1", 79.0, 100.5, 74.0, 78.0, 102.0, 108.0, 2.0),
        ("PestRand", "Z04_Pest2", 124.0, 152.5, 118.0, 122.0, 155.0, 162.0, 2.4),
    ]
    out: list[tuple] = []
    idx = 1
    last_side = rng.choice(("N", "S"))

    for _, _zone, m0, m1, enter, start, end, leave, pinch in zones:
        m = m0 + rng.uniform(-0.8, 0.4)
        while m <= m1:
            if rng.random() < 0.14:
                m += rng.uniform(3.0, 7.0)
                continue

            anchor = round(max(m0, min(m1, m + rng.uniform(-1.6, 1.6))), 2)
            fn, fs = bg.funnel_bounds(anchor, enter, start, end, leave, pinch)
            if fn - fs < bg.MIN_PASSAGE_M:
                m += rng.uniform(2.0, 4.0)
                continue

            cluster = 1 if rng.random() < 0.28 else rng.randint(2, 5)
            for _ in range(cluster):
                if rng.random() < 0.38:
                    side = "S" if last_side == "N" else "N"
                else:
                    side = rng.choice(("N", "S"))
                last_side = side

                xm = round(max(m0, min(m1, anchor + rng.uniform(-1.8, 1.8))), 2)
                fn, fs = bg.funnel_bounds(xm, enter, start, end, leave, pinch)
                if fn - fs < bg.MIN_PASSAGE_M:
                    continue

                y = _pest_y_on_side(rng, xm, fn, fs, side)
                sx = bg.PEST_TILE_SIZE * rng.uniform(0.7, 1.4)
                sy = sx * rng.uniform(0.8, 1.25)
                rot = rng.uniform(-42, 42)
                uid = f"Pesticide_Rand_{idx:03d}"
                out.append((uid, "PesticideCloud", y, xm, round(sx, 3), round(sy, 3), round(rot, 1)))
                idx += 1

            m += rng.uniform(1.2, 3.6) if rng.random() < 0.78 else rng.uniform(4.5, 7.5)

    return out


def kind_for(obj: dict) -> str | None:
    cat = obj["category"]
    comment = obj.get("comment", "")
    sx, sy = float(obj["sx"]), float(obj["sy"])
    if cat in SKIP:
        return None
    if cat == "Spawn":
        return "Spawn"
    if cat in ("Flower", "FlowerExplore"):
        return "Flower"
    if cat == "FlowerWilted":
        return "FlowerWilted"
    if cat in ("Buisson", "PetitePlante"):
        return "DecorPlant"
    if cat == "Branche":
        return "BranchObstacle"
    if cat == "TroncPetit":
        return "TrunkSmall"
    if cat == "TroncGeant":
        return "TrunkGiant"
    if cat == "Goulet":
        return "Goulet"
    if cat == "Entonnoir":
        return "FunnelArc"
    if cat == "LimiteRush":
        return "RushWall"
    if cat == "Bifurcation":
        return None
    if cat == "Nectar":
        return "Nectar"
    if cat == "Hornet":
        if "PATROL" in comment:
            return "HornetPatrol"
        if "CHASE" in comment:
            return "HornetChaseTrigger"
        return "HornetStatic"
    if cat == "Ruche":
        return "Shelter"
    return None


def add(entries: list, uid: str, kind: str, gy: float, gz: float, sx=1.0, sy=1.0, sz=1.0, rot=0.0):
    if kind in SPREAD_KINDS:
        gy, gz = spread_spatial(uid, kind, gy, gz)
    entries.append((uid, kind, 0.0, gy, gz, sx, sy, sz, rot))


def main() -> None:
    bg = load_bg()
    data = json.loads(SCAN.read_text(encoding="utf-8"))
    entries: list[tuple] = []
    seen: set[str] = set()

    for obj in sorted(data, key=lambda o: (o["x"], o["uid"])):
        kind = kind_for(obj)
        if kind is None:
            continue
        uid = obj["uid"]
        m, y = float(obj["x"]), float(obj["y"])
        sx, sy, rot = float(obj["sx"]), float(obj["sy"]), float(obj["rot"])
        if kind == "TrunkGiant":
            y = wy_m(bg, m, 0.0)
        add(entries, uid, kind, y, m, sx, sy, 1.0, rot)
        seen.add(uid)
        if obj["category"] == "Ruche":
            add(entries, "Victory_Trigger", "VictoryTrigger", y, m, 3.0, 4.0, 1.0, 0.0)
            seen.add("Victory_Trigger")

    for uid, kind, y, m, sx, sy, rot in random_pesticides(bg):
        add(entries, uid, kind, y, m, sx, sy, 1.0, rot)

    nectar_idx = 20
    for bif_id, label, m0, m1, zone, reward, nodes in bg.BIFURCATION_SPECS:
        for i, (nm, off_px) in enumerate(nodes):
            off_boost = off_px * 1.35
            y = wy_px(bg, nm, off_boost)
            fid = f"ExploreFlower_{bif_id}_{i+1:02d}"
            if fid not in seen:
                add(entries, fid, "Flower", y, nm, 0.85, 0.85, 1.0, 0.0)
                seen.add(fid)
            nectar_idx += 1
            ny = y + (0.55 if off_px > 0 else -0.55)
            add(entries, f"Nectar_Explore_{nectar_idx:03d}", "Nectar", ny, nm + (i * 0.4), 1.0, 1.0, 1.0, 0.0)

    for i, (bm, off_m) in enumerate(bg.BYPASS_SPECS):
        off_boost = off_m * 1.4 if abs(off_m) > 0.1 else (1.6 if i % 2 else -1.6)
        y = wy_m(bg, bm, off_boost)
        add(entries, f"BypassBranch_{i+1:02d}", "BranchObstacle", y, bm + (i * 0.3), 1.2, 1.8, 1.0, 15.0 if i % 2 else -20.0)
        if i in (1, 3, 5, 7):
            add(entries, f"Nectar_Bypass_{i+1:02d}", "Nectar", y + 0.6, bm + 0.5, 1.0, 1.0, 1.0, 0.0)

    for fid, fm, yoff, zone, has_nectar in bg.FLOWER_SPECS:
        if zone != "Z07_Meadow":
            continue
        y = wy_px(bg, fm, (bg.SAFE_OFF_PX + yoff) * 1.2)
        add(entries, f"MeadowFlower_{fid}", "Flower", y, fm + 0.3, 1.0, 1.0, 1.0, 0.0)
        if has_nectar:
            add(entries, f"MeadowNectar_{fid}", "Nectar", y + 0.55, fm - 0.2, 1.0, 1.0, 1.0, 0.0)

    maze_spots = [
        (108, 3.6, 2.2, 3.5, 0), (118, -3.4, 2.5, 3.0, 90), (132, 4.1, 2.0, 2.8, 0),
        (148, -3.8, 3.0, 2.2, 45), (162, 3.2, 2.0, 3.2, 0), (178, -3.5, 2.5, 3.5, -30),
        (205, 3.8, 2.0, 2.8, 0), (238, -3.1, 2.2, 3.0, 0), (258, 3.4, 2.0, 2.5, 0),
        (312, -3.6, 2.5, 3.0, 15), (328, 3.5, 2.0, 2.5, 0), (355, -3.2, 2.5, 3.0, -20),
        (388, 2.9, 2.0, 2.5, 0), (418, -2.6, 2.0, 2.2, 0), (438, 2.2, 2.0, 2.0, 10),
        (455, -1.9, 2.0, 2.0, -10),
    ]
    for i, (m, yoff, sx, sy, rot) in enumerate(maze_spots):
        add(entries, f"MazeObstacle_{i+1:02d}", "BranchObstacle", wy_m(bg, m, yoff), m + (i % 3) * 0.25, sx, sy, 1.0, rot)

    for i, m in enumerate(range(15, 495, 18)):
        lane = 1 if i % 2 == 0 else -1
        y = wy_m(bg, m, lane * (3.0 + (i % 4) * 0.45))
        add(entries, f"ScatterDecor_{i+1:03d}", "DecorPlant", y, m + (i % 5) * 0.35, 0.6, 0.6, 1.0, 0.0)
        if i % 3 == 0:
            add(entries, f"ScatterNectar_{i+1:03d}", "Nectar", y + lane * 0.4, m - 0.6, 1.0, 1.0, 1.0, 0.0)

    for m, yoff in [(405, 2.8), (420, -2.4), (435, 2.1), (452, -1.9), (468, 1.6)]:
        add(entries, f"Hornet_V_{int(m)}", "HornetStatic", wy_m(bg, m, yoff), m, 1.0, 1.0, 1.0, 0.0)

    entries.sort(key=lambda e: (e[4], e[0]))

    lines = [
        "using System.Collections.Generic;",
        "using UnityEngine;",
        "",
        "/// <summary>Catalogue spatial enrichi — routes multiples · pesticides aléatoires.</summary>",
        "public static class LevelDressingCatalog",
        "{",
        "    public enum Kind",
        "    {",
    ]
    for k in KIND_ENUM:
        lines.append(f"        {k},")
    lines += [
        "    }",
        "",
        "    public readonly struct Entry",
        "    {",
        "        public readonly string Id; public readonly Kind Type;",
        "        public readonly float Gx, Gy, Gz; public readonly Vector3 Scale; public readonly float RotZ;",
        "        public Entry(string id, Kind type, float gx, float gy, float gz,",
        "            float sx = 1f, float sy = 1f, float sz = 1f, float rotZ = 0f)",
        "        { Id = id; Type = type; Gx = gx; Gy = gy; Gz = gz;",
        "          Scale = new Vector3(sx, sy, sz); RotZ = rotZ; }",
        "        public Vector3 WorldPosition => new(Gz, Gy, 0f);",
        "    }",
        "    public static IReadOnlyList<Entry> All => Entries;",
        "    static readonly List<Entry> Entries = Build();",
        "    static List<Entry> Build()",
        "    {",
        "        var e = new List<Entry>();",
    ]
    for uid, kind, gx, gy, gz, sx, sy, sz, rot in entries:
        lines.append(
            f'        e.Add(new Entry("{uid}", Kind.{kind}, {fmt_f(gx)}, {fmt_f(gy)}, {fmt_f(gz)}, '
            f"{fmt_f(sx)}, {fmt_f(sy)}, {fmt_f(sz)}, {fmt_f(rot)}));"
        )
    lines += [f"        return e; // {len(entries)} instances", "    }", "}", ""]
    OUT.write_text("\n".join(lines), encoding="utf-8", newline="\n")
    print(f"Enriched catalog: {len(entries)} entries -> {OUT}")


if __name__ == "__main__":
    main()