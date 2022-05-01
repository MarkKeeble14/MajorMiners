using UnityEngine;

public class DefenderPlayer : Player
{
    protected override void TryPlaceUnit()
    {
        if (!tileCursor.currentTile.Breakable) return;
        
        Instantiate(_unitsToSpawn[currentUnitIndex], tileCursor.currentTile.transform.position, Quaternion.identity);
        ClosePlacementMode();
    }
}
