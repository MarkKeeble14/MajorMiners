using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;

public class AttackerPlayer : Player
{
    public float resourceHealth = 10000;
    private float startResourceHealth;
    [SerializeField] private BetterBarDisplay hpDisplay;
    [SerializeField] private List<UnitPathfind> pathfindingUnits = new List<UnitPathfind>();

    private void Start()
    {
        startResourceHealth = resourceHealth;
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
        money -= unit.Cost;

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
        if (unit.Cost > money) return false;
        if (tileCursor.currentTile.occupyingTower) return false;

        if (!cooldowns[unit.gameObject].IsFinished()) return false;
        
        var coords = tileCursor.coordinates;

        if (!(coords.x < tileManager.AroundCanyon
            || coords.x > tileManager.Rows - 1 - tileManager.AroundCanyon 
            || coords.y < tileManager.AroundCanyon 
            || coords.y > tileManager.Columns - 1 - tileManager.AroundCanyon)) return false;

        return true;
    }

    protected override void Update()
    {
        base.Update();

        hpDisplay.SetSize(resourceHealth, startResourceHealth);

        if (resourceHealth <= 0)
        {
            SceneManager.LoadScene("AttackerWinScreen");
        }

        CheckIfGameOver();
    }


    private void CheckIfGameOver()
    {
        if (spawnedUnits.Count > 0) return;
        foreach (GameObject o in _unitsToSpawn)
        {
            if (baseUnitDictionary[o].Cost < money)
            {
                return;
            }
        }

        // Games over, player cannot afford anything
        SceneManager.LoadScene("DefenderWinScreen");
    }
}