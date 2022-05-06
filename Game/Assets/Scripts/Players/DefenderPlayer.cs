using FMODUnity;
using Grid;
using UnityEngine;

public class DefenderPlayer : Player
{
    [SerializeField] private AttackerPlayer attacker;
    [SerializeField] private KeyCode sellTower;
    [SerializeField] private GameObject sellTowerParticles;
    [SerializeField] private Color placeColor;
    [SerializeField] private Color sellColor;


    protected override void TryPlaceUnit()
    {
        if (!CanPlaceUnit()) return;

        BaseUnit unit = _unitsToSpawn[currentUnitIndex].GetComponent<BaseUnit>();
        cooldowns[unit.gameObject].Reset();

        AlterMoney(-unit.Cost, tileCursor.currentTile.transform.position);
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
        if (unit.Cost > Money) return false;
        if (tileCursor.currentTile.occupyingTower) return false;
        if (!cooldowns[unit.gameObject].IsFinished()) return false;
        return true;
    }

    public override void UpdatePlayer()
    {
        base.UpdatePlayer();

        if (Input.GetKeyDown(sellTower))
        {
            GameObject tower;
            WorldTile t = tileCursor.currentTile;
            if ((tower = t.occupyingTower) != null)
            {
                spawnedUnits.Remove(tower);
                t.SetTower(null);
                t.SetBreakable(true);
                FindObjectOfType<DefenderPlayer>().AlterMoney(tower.GetComponent<BaseUnit>().Cost / 5, tower.transform.position);
                RuntimeManager.PlayOneShot("event:/SFX/Digging");
                Instantiate(sellTowerParticles, t.transform.position, Quaternion.identity);
                attacker.UpdatePaths();
            }
        }

        if (tileCursor.currentTile.occupyingTower != null)
        {
            tileCursorSpriteRenderer.color = sellColor;
        } else
        {
            if (CanPlaceUnit())
            {
                tileCursorSpriteRenderer.color = placeColor;
            } else
            {
                tileCursorSpriteRenderer.color = placeColor;
            }
        }
    }
}
