using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Player attacker;
    [SerializeField] private GameObject attackerTileCursor;
    [SerializeField] private KeyCode attackerExitPlacementMode;

    private void Update()
    {
        if (Input.GetKeyDown(attackerExitPlacementMode))
        {
            AttackerClosePlacementMode();
        }

        if (attacker.placing)
        {
            // 
        }
    }

    private void AttackerOpenPlacementMode()
    {
        attackerTileCursor.SetActive(true);
    }

    private void AttackerClosePlacementMode()
    {
        attackerTileCursor.SetActive(false);
    }

    public void AttackerOpenMultiPlacementMode(ShopUnit unit, int number)
    {
        // Attacker does not have enough money to place the units
        if (attacker.money < unit.Cost * number)
            return;
        AttackerOpenPlacementMode();
    }

    public void AttackerOpenSinglePlacementMode(ShopUnit unit)
    {
        // Attacker does not have enough money to place the unit
        if (attacker.money < unit.Cost)
            return;
        AttackerOpenPlacementMode();
    }
}

[System.Serializable]
public class Player
{
    public int money;
    public bool placing;
}



[System.Serializable]
public class ShopUnit
{
    [SerializeField] private string name;
    [SerializeField] private GameObject prefab;
    [SerializeField] private int cost;
    public int Cost
    {
        get { return cost; }
    }
    [SerializeField] private PlacementType[] placementType;
}

public enum PlacementType
{
    WALKABLE_TILE,
    BLOCKED_TILE
}