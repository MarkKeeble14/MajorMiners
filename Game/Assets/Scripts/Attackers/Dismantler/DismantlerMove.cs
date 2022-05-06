using System.Collections;
using System.Collections.Generic;
using System.IO;
using FMODUnity;
using Grid;
using UnityEngine;

public class DismantlerMove : UnitMove
{
    [SerializeField] protected TileManager tileManager;
    [SerializeField] protected float timeToMine = 1f;
    [SerializeField] protected int numTowersCanBreak = 1;
    private int currentNumberTowersBroken;

    protected override IEnumerator MoveToEachPosition()
    {
        for (int i = 0; i < path.Count; i++)
        {
            if (path[i].isTower)
            {
                WorldTile t = tileManager.GetTile(path[i].gridX, grid.gridSizeY - path[i].gridY - 1);
                if (!t.occupyingTower)
                {
                    uPathfind.FindNewPath();
                    break;
                }
                BaseDefender tower = t.occupyingTower.GetComponent<BaseDefender>();
                tower.SetDismantleBar(timeToMine);

                yield return new WaitForSeconds(timeToMine);

                if (!tower)
                {
                    uPathfind.FindNewPath();
                    break;
                } else
                {
                    tower.HideDismantleBar();

                    // Successfully broke tower
                    FindObjectOfType<AttackerPlayer>().AlterMoney(tower.Cost / 4, t.transform.position);
                    t.SetTower(null);
                    t.SetBreakable(true);

                    if (++currentNumberTowersBroken > numTowersCanBreak - 1)
                    {
                        onRoute = false;

                        // Spawn particles
                        Instantiate(resourceEffect, transform.position, Quaternion.identity);

                        // Die
                        Destroy(gameObject);
                        bAttacker.onDeath();
                    }
                    uPathfind.FindNewPath();
                    break;
                }
            }
            yield return MoveTo(path[i].worldPosition);
        }
    }
}
