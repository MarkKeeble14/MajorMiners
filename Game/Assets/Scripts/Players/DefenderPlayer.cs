using FMODUnity;
using UnityEngine;

public class DefenderPlayer : Player
{
    [SerializeField] private AttackerPlayer attacker;

    protected override void TryPlaceUnit()
    {
        if (!CanPlaceUnit()) return;

        BaseUnit unit = _unitsToSpawn[currentUnitIndex].GetComponent<BaseUnit>();
        cooldowns[unit.gameObject].Reset();

        money -= unit.Cost;
        RuntimeManager.PlayOneShot("event:/SFX/Tower_Deploy", tileCursor.currentTile.transform.position);
        
        GameObject spawn = Instantiate(_unitsToSpawn[currentUnitIndex], tileCursor.currentTile.transform.position, Quaternion.identity);
        spawnedUnits.Add(spawn);
        tileCursor.currentTile.SetTower(spawn);

        // Update the paths of existing enemies
        attacker.UpdatePaths();

        placedUnit = true;
    }

    protected override bool CanPlaceUnit()
    {
        if (!tileCursor.currentTile.Breakable) return false;

        var unit = _unitsToSpawn[currentUnitIndex].GetComponent<BaseUnit>();
        if (unit.Cost > money) return false;
        if (tileCursor.currentTile.occupyingTower) return false;
        if (!cooldowns[unit.gameObject].IsFinished()) return false;
        return true;
    }
}
