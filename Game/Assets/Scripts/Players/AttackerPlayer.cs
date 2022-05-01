using UnityEngine;

public class AttackerPlayer : Player
{
    protected override void TryPlaceUnit()
    {
        if (tileCursor.currentTile.Breakable) return;
        
        var unit = _unitsToSpawn[currentUnitIndex].GetComponent<BaseUnit>();
        if (unit.Cost > money) return;

        money -= unit.Cost;
        
        placedUnit = true;
        Instantiate(_unitsToSpawn[currentUnitIndex], new Vector3(tileCursor.currentTile.transform.position.x, tileCursor.currentTile.transform.position.y, 0), Quaternion.identity);
    }
}
