using UnityEngine;

public class AttackerPlayer : Player
{
    protected override void TryPlaceUnit()
    {
        if (tileCursor.currentTile.Breakable) return;
        
        var unit = _unitsToSpawn[currentUnitIndex].GetComponent<BaseUnit>();
        if (unit.Cost > money) return;
        if (tileCursor.currentTile.occupyingTower) return;

        money -= unit.Cost;
        
        Instantiate(_unitsToSpawn[currentUnitIndex], tileCursor.currentTile.transform.position, Quaternion.identity);
        placedUnit = true;
    }
}
