using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using Grid;
using UnityEngine;

public class MinerMove : UnitMove
{
    [SerializeField] public TileManager tileManager;
    [SerializeField] float timeToMine = 1.0f;
    [SerializeField] private GameObject dirtEffect;
    private AttackerPlayer attacker;
    private BaseAttacker baseAttacker;
    private MinerPathfind pathfind;

    private new void Awake()
    {
        base.Awake();
        attacker = FindObjectOfType<AttackerPlayer>();
        baseAttacker = GetComponent<BaseAttacker>();
        baseAttacker.onDeath += () => StopAllCoroutines();
        pathfind = GetComponent<MinerPathfind>();
    }

    protected override IEnumerator MoveToEachPosition()
    {
        for (int i = 0; i < path.Count; i++)
        {
            if (path[i].breakable)
            {
                WorldTile t = tileManager.GetTile(path[i].gridX, grid.gridSizeY - path[i].gridY - 1);
                t.AddActiveMiner(this);
                baseAttacker.onDeath += () => t.RemoveActiveMiner(this);
                yield return new WaitUntil(() => t.IsMined);
                attacker.DesignatedTileMined(t);

                Instantiate(dirtEffect, t.transform.position, Quaternion.identity);
                RuntimeManager.PlayOneShot("event:/SFX/Digging");
                t.BreakBreakable();
            }
            yield return MoveTo(path[i].worldPosition);
        }

        onRoute = false;

        // Reached end of path
        WorldTile center = tileManager.GetTile(tileManager.Rows / 2, tileManager.Columns / 2);
        if (transform.position.x == center.transform.position.x 
            && transform.position.y == center.transform.position.y)
        {
            // Spawn particles
            Instantiate(resourceEffect, transform.position, Quaternion.identity);

            // Add Money and subtract resources
            attacker.AlterMoney(GetComponent<BaseUnit>().Cost, transform.position);
            attacker.resourceHealth -= GetComponent<BaseUnit>().Damage;

            // Play sounds
            RuntimeManager.PlayOneShot("event:/SFX/Mining");
            RuntimeManager.PlayOneShot("event:/SFX/Human_Cheer");

            // Die!
            bAttacker.onDeath();
            Destroy(gameObject);
        } else
        {
            pathfind.FindNewPath();
        }
    }
}