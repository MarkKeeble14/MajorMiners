using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class to represent a world position.
public class Node
{
    private const int BreakableWeightIncrease = 30;
    public bool walkable;
    public bool breakable;
    public Vector3 worldPosition;
    // Position of node in the array of nodes
    public int gridX;
    public int gridY;

    public int gCost;
    public int hCost;
    // Keeps track of parent node/last node (relative path it takes, parent node is the node before it in the path)
    public Node parent;

    public int row;
    public int column;

    public Node(bool _walkable, Vector3 _worldPos, int _gridX, int _gridY)
    {
        walkable = _walkable;
        worldPosition = _worldPos;
        gridX = _gridX;
        gridY = _gridY;
    }

    public int fCost()
    {
        if (breakable) {
            return gCost + hCost + BreakableWeightIncrease;
        } else
        {
            return gCost + hCost;
        }
    }
}
