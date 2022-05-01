using UnityEngine;
using UnityEngine.SceneManagement;

public class AttackerPlayer : Player
{
    public float resourceHealth = 10000;

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

    protected override void Update()
    {
        base.Update();
        
        if (resourceHealth <= 0)
        {
            SceneManager.LoadScene("AttackerWinScreen");
        }
    }
}