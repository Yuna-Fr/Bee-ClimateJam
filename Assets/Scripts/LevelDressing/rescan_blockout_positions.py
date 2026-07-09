#!/usr/bin/env python3
"""Recalcule Y pixel-exact depuis blockout.png pour éparpiller le décor."""
from __future__ import annotations

import json
import re
import subprocess
import sys
from pathlib import Path

import numpy as np
from PIL import Image

ROOT = Path(__file__).resolve().parents[4]
BLOCKOUT = ROOT / "Etapes" / "blockout.png"
SCAN_JSON = ROOT / "Production" / "LEVEL_BLOCKOUT_SCAN.json"
OUT_JSON = ROOT / "Production" / "LEVEL_BLOCKOUT_SCAN_RECAL.json"
CATALOG_GEN = Path(__file__).resolve().parent / "generate_catalog_from_blockout.py"

LEVEL_M = 500.0
CORRIDOR_H = 10.0

PATH_KF = [(0, 0), (100, -0.03), (200, 0.04), (274, -0.07), (350, 0.1), (400, 0.14), (500, 0.2)]


def find_corridor(arr: np.ndarray) -> tuple[int, int]:
    h, w, _ = arr.shape
    red = (arr[:, :, 0] > 200) & (arr[:, :, 1] < 60) & (arr[:, :, 2] < 60)
    rows = np.where(red.sum(axis=1) > w * 0.04)[0]
    if len(rows) >= 2:
        return int(rows[0]), int(rows[-1])
    return int(h * 0.48), int(h * 0.64)


def find_x_bounds(arr: np.ndarray, north: int, south: int) -> tuple[float, float]:
    play = arr[north:south, :]
    active = np.abs(play.astype(int) - 255).sum(axis=2) > 40
    cols = np.where(active.any(axis=0))[0]
    if len(cols) < 2:
        return 500.0, float(arr.shape[1] - 500)
    return float(cols[0]), float(cols[-1])


def path_py(arr: np.ndarray, x: int, north: int, south: int) -> float | None:
    col = arr[north:south, max(0, min(x, arr.shape[1] - 1))]
    blue = (
        (col[:, 2] > 130)
        & (col[:, 0] < 140)
        & (col[:, 1] < 210)
        & (col[:, 2] > col[:, 0] + 15)
    )
    ys = np.where(blue)[0]
    if len(ys) < 6:
        return None
    return north + float(np.median(ys))


def path_y_keyframe(m: float) -> float:
    if m <= PATH_KF[0][0]:
        return PATH_KF[0][1]
    for i in range(len(PATH_KF) - 1):
        m0, y0 = PATH_KF[i]
        m1, y1 = PATH_KF[i + 1]
        if m0 <= m <= m1:
            t = (m - m0) / (m1 - m0) if m1 != m0 else 0.0
            return y0 + t * (y1 - y0)
    return PATH_KF[-1][1]


def world_y_from_pixel(cy: float, m: float, arr: np.ndarray, north: int, south: int, x0: float, ppx: float, ppy: float, ref_py: float) -> float:
    x_px = int(x0 + m * ppx)
    py_path = path_py(arr, x_px, north, south)
    if py_path is None:
        return round(path_y_keyframe(m) + (cy - ref_py) / ppy, 4)
    offset_m = (cy - py_path) / ppy
    base = (py_path - ref_py) / ppy
    return round(base + offset_m, 4)


def blob_y_near(arr, obj, north, south, x0, ppx, ppy, ref_py):
    m = float(obj["x"])
    cat = obj["category"]
    x_px = int(x0 + m * ppx)
    half = max(18, int(12 * ppx / 5.8))
    x1, x2 = max(0, x_px - half), min(arr.shape[1], x_px + half)
    patch = arr[north:south, x1:x2]
    r, g, b = patch[:, :, 0], patch[:, :, 1], patch[:, :, 2]

    if cat in ("Flower", "FlowerExplore", "FlowerWilted"):
        mask = (g > 95) & (r < 125) & (b < 125) & (g > r + 12)
    elif cat == "Nectar":
        mask = (r > 195) & (g > 140) & (b < 130)
    elif cat in ("Branche", "TroncPetit", "TroncGeant"):
        mask = (r > 45) & (r < 225) & (g < 155) & (b < 135) & (r > g + 8)
    elif cat in ("Buisson", "PetitePlante"):
        mask = ((g > 30) & (g < 110) & (r < 80) & (b < 80) & (g > r + 5)) | (
            (g > 120) & (r > 95) & (b > 85) & (r < 210) & (g > b)
        )
    elif cat == "Hornet":
        mask = (r > 195) & (g < 95) & (b < 95)
    elif cat == "Ruche":
        mask = (r > 45) & (r < 225) & (g < 165) & (b < 145) & (r > g)
    elif cat == "Pesticide":
        mask = (r > 90) & (b > 110) & (g < 95)
    else:
        return None

    ys, xs = np.where(mask)
    if len(xs) == 0:
        return None
    wx = xs + x1
    idx = int(np.argmin(np.abs(wx - x_px)))
    cy = north + float(ys[idx])
    return world_y_from_pixel(cy, m, arr, north, south, x0, ppx, ppy, ref_py)


def main() -> None:
    arr = np.array(Image.open(BLOCKOUT).convert("RGB"))
    north, south = find_corridor(arr)
    ppy = (south - north) / CORRIDOR_H
    x0, x1 = find_x_bounds(arr, north, south)
    ppx = (x1 - x0) / LEVEL_M
    ref_py = path_py(arr, int(x0 + 100 * ppx), north, south) or (north + south) / 2

    print(f"blockout {arr.shape[1]}x{arr.shape[0]}")
    print(f"corridor {north}-{south}  PX_PER_M_Y={ppy:.2f}")
    print(f"x {x0:.0f}-{x1:.0f}  PX_PER_M_X={ppx:.2f}")

    data = json.loads(SCAN_JSON.read_text(encoding="utf-8"))
    spawn_cats = {
        "Spawn", "Flower", "FlowerExplore", "FlowerWilted", "Buisson", "PetitePlante",
        "Branche", "TroncPetit", "TroncGeant", "Nectar", "Hornet", "Ruche",
    }

    updated = 0
    for obj in data:
        if obj["category"] not in spawn_cats:
            continue
        old = float(obj["y"])
        ny = blob_y_near(arr, obj, north, south, x0, ppx, ppy, ref_py)
        if ny is None:
            m_off = re.search(r"offset_y=([+-]?\d+\.?\d*)\s*m", obj.get("comment", ""))
            if m_off:
                ny = round(path_y_keyframe(float(obj["x"])) + float(m_off.group(1)), 4)
            else:
                continue
        if abs(ny - old) > 0.02:
            obj["y"] = ny
            updated += 1
            print(f"  {obj['uid']}: {old:.3f} -> {ny:.3f}")

    OUT_JSON.write_text(json.dumps(data, indent=2, ensure_ascii=False), encoding="utf-8")
    print(f"Updated {updated} positions -> {OUT_JSON}")

    gen = CATALOG_GEN.read_text(encoding="utf-8")
    if "LEVEL_BLOCKOUT_SCAN_RECAL.json" not in gen:
        gen = gen.replace("LEVEL_BLOCKOUT_SCAN.json", "LEVEL_BLOCKOUT_SCAN_RECAL.json")
        CATALOG_GEN.write_text(gen, encoding="utf-8")

    subprocess.run([sys.executable, str(CATALOG_GEN)], check=True)


if __name__ == "__main__":
    main()