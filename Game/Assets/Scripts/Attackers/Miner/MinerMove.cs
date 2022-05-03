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

    private new void Awake()
    {
        base.Awake();
        attacker = FindObjectOfType<AttackerPlayer>();
    }

    protected override IEnumerator MoveToEachPosition()
    {
        for (int i = 0; i < path.Count; i++)
        {
            if (path[i].breakable)
            {
                yield return new WaitForSeconds(timeToMine);
                WorldTile current = tileManager.GetTile(path[i].gridX, grid.gridSizeY - path[i].gridY - 1);
                Instantiate(dirtEffect, current.transform.position, Quaternion.identity);
                RuntimeManager.PlayOneShot("event:/SFX/Digging");
                current.BreakBreakable();
            }
            yield return MoveTo(path[i].worldPosition);
        }
        onRoute = false;

        // Spawn particles
        Instantiate(resourceEffect, transform.position, Quaternion.identity);

        // Add Money and subtract resources
        attacker.money += GetComponent<BaseUnit>().Cost;
        attacker.resourceHealth -= GetComponent<BaseUnit>().Damage;

        // Play sounds
        RuntimeManager.PlayOneShot("event:/SFX/Mining");
        RuntimeManager.PlayOneShot("event:/SFX/Human_Cheer");

        // Die!
        bAttacker.onDeath();
        Destroy(gameObject);
    }
}