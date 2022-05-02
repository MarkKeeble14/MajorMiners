using FMODUnity;
using UnityEngine;

public class AttackerPlayer : Player
{
    public float resourceHealth = 10000;

    protected override void TryPlaceUnit()
    {
        if (tileCursor.currentTile.Breakable) return;
        
        var unit = _unitsToSpawn[currentUnitIndex].GetComponent<BaseUnit>();
        if (unit.Cost > money) return;
        if (tileCursor.currentTile.occupyingTower) return;
        var coords = tileCursor.coordinates;

        if (!(coords.x == 0 || coords.x == tileManager.Rows - 1 || coords.y == 0 || coords.y == tileManager.Columns - 1)) return;

        money -= unit.Cost;
        
        RuntimeManager.PlayOneShot("event:/SFX/Deploy_Human", tileCursor.currentTile.transform.position);
        
        Instantiate(_unitsToSpawn[currentUnitIndex], tileCursor.currentTile.transform.position, Quaternion.identity);
        placedUnit = true;
    }

    protected override void Update()
    {
        base.Update();
        
        if (resourceHealth <= 0)
        {

        }
    }
}