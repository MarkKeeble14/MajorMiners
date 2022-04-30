using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Player attacker;

    private void Update()
    {
    }
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