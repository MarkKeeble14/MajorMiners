using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;
using Grid;

public class AttackerPlayer : Player
{
    public float resourceHealth;
    private float startResourceHealth = 10000;
    [SerializeField] private BetterBarDisplay hpDisplay;
    [SerializeField] private List<UnitPathfind> pathfindingUnits = new List<UnitPathfind>();
    [SerializeField] private ResultsScreenController resultsScreen;
    private List<WorldTile> tilesDesignatedToMine = new List<WorldTile>();
    [SerializeField] private KeyCode designateMine;

    public bool HasTilesDesignatedToMine
    {
        get { return tilesDesignatedToMine.Count > 0; }
    }

    public void DesignatedTileMined(WorldTile t)
    {
        tilesDesignatedToMine.Remove(t);
    }

    public WorldTile GetClosestDesignatedTile(Transform origin)
    {
        WorldTile toReturn = null;
        foreach (WorldTile t in tilesDesignatedToMine)
        {
            if (toReturn == null)
            {
                toReturn = t;
            } else
            {
                if (Vector3.Distance(origin.position, t.transform.position) < Vector3.Distance(origin.position, toReturn.transform.position))
                {
                    toReturn = t;
                }
            }
        }
        return toReturn;
    }

    public new void ResetPlayer()
    {
        base.ResetPlayer();
        pathfindingUnits.Clear();
        resourceHealth = startResourceHealth;
    }

    protected override void TryPlaceUnit()
    {
        if (!CanPlaceUnit()) return;

        BaseUnit unit = _unitsToSpawn[currentUnitIndex].GetComponent<BaseUnit>();
        cooldowns[unit.gameObject].Reset();

        // Spawn Unit
        GameObject obj = Instantiate(_unitsToSpawn[currentUnitIndex], tileCursor.currentTile.transform.position, Quaternion.identity);
        BaseAttacker bAttacker = obj.GetComponent<BaseAttacker>();
        UnitPathfind uPF = obj.GetComponent<UnitPathfind>();
        RuntimeManager.PlayOneShot("event:/SFX/Deploy_Human", tileCursor.currentTile.transform.position);
        AlterMoney(-unit.Cost, tileCursor.currentTile.transform.position);

        // Keeping track of spawned units
        spawnedUnits.Add(obj);
        // Keeping track of units which require pathfinding
        pathfindingUnits.Add(uPF);
        // Remove Units from both lists when they die
        bAttacker.onDeath += (() => {
            spawnedUnits.Remove(obj);
            pathfindingUnits.Remove(uPF);
        });

        placedUnit = true;
    }

    public void UpdatePaths()
    {
        // Might throw an error if an enemy dies while looping
        // If that happens, just redo the update
        try
        {
            foreach (UnitPathfind pathfind in pathfindingUnits)
            {
                pathfind.FindNewPath();
            }
        } catch
        {
            UpdatePaths();
        }
    }
        
    protected override bool CanPlaceUnit()
    {
        if (tileCursor.currentTile.Breakable) return false;

        BaseUnit unit = _unitsToSpawn[currentUnitIndex].GetComponent<BaseUnit>();
        
        if (unit.Cost > Money) return false;
        if (tileCursor.currentTile.occupyingTower) return false;
        if (!cooldowns[unit.gameObject].IsFinished()) return false;
        
        var coords = tileCursor.coordinates;

        if (!(coords.x < tileManager.AroundCanyon
            || coords.x > tileManager.Rows - 1 - tileManager.AroundCanyon 
            || coords.y < tileManager.AroundCanyon 
            || coords.y > tileManager.Columns - 1 - tileManager.AroundCanyon)) return false;

        return true;
    }

    public override void UpdatePlayer()
    {
        base.UpdatePlayer();

        hpDisplay.SetSize(resourceHealth, startResourceHealth);

        if (resourceHealth <= 0)
        {
            resultsScreen.AttackerWon(VICTORY_METHOD.ORE_DEPLETED);
        }

        if (Input.GetKeyDown(designateMine))
        {
            if (tileCursor.currentTile.Breakable)
            {
                if (tilesDesignatedToMine.Contains(tileCursor.currentTile))
                {
                    tilesDesignatedToMine.Remove(tileCursor.currentTile);
                    tileCursor.currentTile.DesignatedToMine = false;
                } else
                {
                    tilesDesignatedToMine.Add(tileCursor.currentTile);
                    tileCursor.currentTile.DesignatedToMine = true;
                }
                tileCursor.currentTile.SetColor();
            }
        }

        CheckIfGameOver();
    }


    private void CheckIfGameOver()
    {
        if (spawnedUnits.Count > 0) return;
        foreach (GameObject o in _unitsToSpawn)
        {
            if (baseUnitDictionary[o].Cost < Money)
            {
                return;
            }
        }

        // Games over, player cannot afford anything
        resultsScreen.DefenderWon(VICTORY_METHOD.ATTACKER_OUT_OF_RESOURCES);
    }
}