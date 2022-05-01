using UnityEngine;

public class AttackerPlayer : Player
{
    protected override void TryPlaceUnit()
    {
        if (tileCursor.currentTile.Breakable) return;
        
        Instantiate(_unitsToSpawn[currentUnitIndex], tileCursor.currentTile.transform.position, Quaternion.identity);
        ClosePlacementMode();
    }
}
