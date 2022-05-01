using UnityEngine;

public class AttackerPlayer : Player
{
    public float resourceHealth = 10000;
    public Grid.TileManager tileManager;

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

    private void Update()
    {
        if (resourceHealth <= 0)
        {

        }
    }
}
