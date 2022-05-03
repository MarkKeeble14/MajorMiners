using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class LooterMove : UnitMove
{
    protected override IEnumerator MoveToEachPosition()
    {
        for (int i = 0; i < path.Count; i++)
        {
            yield return MoveTo(path[i].worldPosition);
        }
        onRoute = false;

        // Spawn particles
        Instantiate(resourceEffect, transform.position, Quaternion.identity);

        // Add Money and subtract resources
        FindObjectOfType<AttackerPlayer>().money += GetComponent<BaseUnit>().Cost;
        FindObjectOfType<AttackerPlayer>().resourceHealth -= GetComponent<BaseUnit>().Damage;

        // Play sounds
        RuntimeManager.PlayOneShot("event:/SFX/Mining");
        RuntimeManager.PlayOneShot("event:/SFX/Human_Cheer");

        // Die!
        Destroy(gameObject);
        bAttacker.onDeath();
    }
}