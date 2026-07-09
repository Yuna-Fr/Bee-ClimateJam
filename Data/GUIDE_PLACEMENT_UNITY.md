# Guide de Placement Pur — Unity (0–500 m)

**Projet :** `Bee-ClimateJam` · branche `game-design`  
**Méthode :** Set dressing technique · 1 prefab = 1 instance · aucun regroupement

## Mapping coordonnées GDD → Unity 2D

| GDD | Unity (monde) |
|-----|----------------|
| **Z** (0–500 m, progression) | **X** |
| **Y** (0–10, hauteur) | **Y** (base) |
| **X** (-5..+5, latéral) | **Y + X** (décalage vertical écran) |

Exemple : GDD `(X:-3, Y:5, Z:80)` → Unity `(80, 2, 0)`

## Prefabs utilisés (projet actuel)

| GDD (guide) | Prefab réel |
|-------------|-------------|
| SpawnPoint | `P_Bee` |
| Prefab_Fleur_Native | `P_Flower` |
| Prefab_Branche_* / Labyrinthe / Tronc | `P_Obstacle_Branch` (+ scale/rotation) |
| Prefab_Frelon_* | `P_Hornet` |
| Prefab_Nuage_Pesticide / Sol toxique | `P_PesticideZone` |
| Prefab_Abri_Buche | `P_Obstacle_Branch` (scale 2.5×1.5) |
| Prefab_Trigger_Victoire | Trigger + `VictoryZone.cs` |

## Génération automatique

1. Ouvrir `Assets/Scenes/Proto1.unity`
2. Menu **Bee Climate Jam → Build Level Dressing (0-500m)**
3. Vérifier la hiérarchie `LevelDressing` (≈70 instances)

Ou : ajouter `LevelDressingSpawner` à la scène · cocher les 5 prefabs · Play.

## Règle absolue

- **Interdit :** « plusieurs fleurs », « quelques branches », « divers décors », « etc. »
- **Obligatoire :** 1 ligne · 1 ID · 1 position · 1 fiche par symbole blockout
- Catalogue complet : `Assets/Scripts/LevelDressing/LevelDressingCatalog.cs`

## Sections

| Section | Z (m) | Objets clés |
|---------|-------|-------------|
| 1 Apprentissage | 0–100 | Fleurs + branches basse/haute |
| 2 Labyrinthe | 100–250 | 4 mazes · tronc · bifurcation haut/bas |
| 3 Dangers | 250–400 | Frelons · pesticides · fleurs secours |
| 4 Abri | 400–500 | Sol toxique · bouquet final · Ruche Z=500 |