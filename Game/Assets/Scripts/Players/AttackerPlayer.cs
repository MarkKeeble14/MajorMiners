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

        money -= unit.Cost;
        
        placedUnit = true;
        Instantiate(_unitsToSpawn[currentUnitIndex], new Vector3(tileCursor.currentTile.transform.position.x, tileCursor.currentTile.transform.position.y, 0), Quaternion.identity);
    }

    private void Update()
    {
        if (resourceHealth <= 0)
        {
            tileManager.GetTile(tileManager.Rows/2, tileManager.Columns/2).SetAsteroid(false);
        }
    }
}
