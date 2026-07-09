#!/usr/bin/env python3
"""Génère LevelDressingCatalog.cs depuis LEVEL_BLOCKOUT_SCAN_RECAL.json (positions blockout exactes)."""
from __future__ import annotations

import json
from pathlib import Path

SCAN = Path(__file__).resolve().parents[4] / "Production" / "LEVEL_BLOCKOUT_SCAN_RECAL.json"
OUT = Path(__file__).resolve().parent / "LevelDressingCatalog.cs"

SKIP = {
    "RouteSAFE", "RouteExplore", "RouteMarker", "LimiteNord", "LimiteSud",
    "LimiteRush", "Checkpoint", "Sol", "FlowerGroup", "Entonnoir", "Goulet",
    "Bifurcation", "Plateforme",
}

KIND_ENUM = [
    "Spawn", "Flower", "FlowerWilted", "DecorPlant", "BranchObstacle",
    "TrunkSmall", "TrunkGiant", "Nectar", "PesticideCloud", "PesticideFloor",
    "HornetPatrol", "HornetStatic", "HornetChaseTrigger", "Shelter", "VictoryTrigger",
]


def kind_for(obj: dict) -> str | None:
    cat = obj["category"]
    uid = obj["uid"]
    comment = obj.get("comment", "")
    sx, sy = float(obj["sx"]), float(obj["sy"])

    if cat in SKIP:
        return None
    if cat == "Spawn":
        return "Spawn"
    if cat == "Flower":
        return "Flower"
    if cat == "FlowerExplore":
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
    if cat == "Nectar":
        return "Nectar"
    if cat == "Pesticide":
        return "PesticideFloor" if sx >= 5 or sy >= 3 else "PesticideCloud"
    if cat == "Hornet":
        if "PATROL" in comment:
            return "HornetPatrol"
        if "CHASE" in comment:
            return "HornetChaseTrigger"
        return "HornetStatic"
    if cat == "Ruche":
        return "Shelter"
    return None


def fmt_f(v: float) -> str:
    if v == int(v):
        return f"{int(v)}f"
    return f"{v:.4g}".rstrip("0").rstrip(".") + "f"


def main() -> None:
    data = json.loads(SCAN.read_text(encoding="utf-8"))
    entries: list[tuple[str, str, float, float, float, float, float, float, float]] = []

    for obj in sorted(data, key=lambda o: (o["x"], o["uid"])):
        kind = kind_for(obj)
        if kind is None:
            continue
        entries.append((
            obj["uid"],
            kind,
            0.0,
            float(obj["y"]),
            float(obj["x"]),
            float(obj["sx"]),
            float(obj["sy"]),
            1.0,
            float(obj["rot"]),
        ))
        if obj["category"] == "Ruche":
            entries.append((
                "Victory_Trigger",
                "VictoryTrigger",
                0.0,
                float(obj["y"]),
                float(obj["x"]),
                3.0, 4.0, 1.0, 0.0,
            ))

    lines = [
        "using System.Collections.Generic;",
        "using UnityEngine;",
        "",
        "/// <summary>",
        "/// Catalogue blockout verrouillé — LEVEL_BLOCKOUT_SCAN_RECAL.json (271 objets source).",
        "/// Coordonnées : Z(m) = progression X Unity · Y = hauteur monde.",
        "/// </summary>",
        "public static class LevelDressingCatalog",
        "{",
        "    public enum Kind",
        "    {",
    ]
    for i, k in enumerate(KIND_ENUM):
        lines.append(f"        {k},")
    lines += [
        "    }",
        "",
        "    public readonly struct Entry",
        "    {",
        "        public readonly string Id;",
        "        public readonly Kind Type;",
        "        public readonly float Gx, Gy, Gz;",
        "        public readonly Vector3 Scale;",
        "        public readonly float RotZ;",
        "",
        "        public Entry(string id, Kind type, float gx, float gy, float gz,",
        "            float sx = 1f, float sy = 1f, float sz = 1f, float rotZ = 0f)",
        "        {",
        "            Id = id; Type = type; Gx = gx; Gy = gy; Gz = gz;",
        "            Scale = new Vector3(sx, sy, sz); RotZ = rotZ;",
        "        }",
        "",
        "        public Vector3 WorldPosition => new(Gz, Gy, 0f);",
        "    }",
        "",
        "    public static IReadOnlyList<Entry> All => Entries;",
        "    static readonly List<Entry> Entries = Build();",
        "",
        "    static List<Entry> Build()",
        "    {",
        "        var e = new List<Entry>();",
    ]

    for uid, kind, gx, gy, gz, sx, sy, sz, rot in entries:
        lines.append(
            f'        e.Add(new Entry("{uid}", Kind.{kind}, {fmt_f(gx)}, {fmt_f(gy)}, {fmt_f(gz)}, '
            f"{fmt_f(sx)}, {fmt_f(sy)}, {fmt_f(sz)}, {fmt_f(rot)}));"
        )

    lines += [
        f"        return e; // {len(entries)} instances spawnables",
        "    }",
        "}",
        "",
    ]

    OUT.write_text("\n".join(lines), encoding="utf-8", newline="\n")
    print(f"Wrote {len(entries)} entries -> {OUT}")


if __name__ == "__main__":
    main()