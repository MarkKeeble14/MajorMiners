using UnityEngine;

public class DefenderPlayer : Player
{
    protected override void TryPlaceUnit()
    {
        if (!tileCursor.currentTile.Breakable) return;
        
        var unit = _unitsToSpawn[currentUnitIndex].GetComponent<BaseUnit>();
        if (unit.Cost > money) return;

        money -= unit.Cost;
        
        Instantiate(_unitsToSpawn[currentUnitIndex], tileCursor.currentTile.transform.position, Quaternion.identity);
        placedUnit = true;
    }
}
