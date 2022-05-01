using UnityEngine;

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
