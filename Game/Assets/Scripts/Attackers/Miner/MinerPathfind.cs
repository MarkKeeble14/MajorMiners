using System.Collections;
using System.Collections.Generic;
using Grid;
using UnityEngine;

public class MinerPathfind : UnitPathfind
{
    private AttackerPlayer attacker;
    private bool targetingAsteroid;
    // (FOR TESTING)
    void Update()
    {
        FindPath(seeker.position, target.position);
    }

    public override void FindTarget()
    {
        if (attacker.HasTilesDesignatedToMine)
        {
            target = attacker.GetClosestDesignatedTile(transform).transform;
            targetingAsteroid = false;
        } else
        {
            target = tileManager.GetTile(tileManager.Rows / 2, tileManager.Columns / 2).transform;
            targetingAsteroid = true;
        }
    }

    public bool TargetIsAsteroid
    {
        get {
            return targetingAsteroid;
        }
    }

    // Gets the grid from the MyGrid Script
    protected override void Awake()
    {
        base.Awake();
        attacker = FindObjectOfType<AttackerPlayer>();
        FindTarget();
    }

    // Refer to the PSEUDO-CODE in A* algorithm notes
    public override void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

        // Set of nodes to be evaluated
        List<Node> openSet = new List<Node>();
        // Set of nodes already evaluated
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost() < currentNode.fCost() || openSet[i].fCost() == currentNode.fCost() && openSet[i].hCost < currentNode.hCost)
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                RetracePath(startNode, targetNode);
                return;
            }

            foreach (Node neighbour in grid.GetNeighbours(currentNode))
            {
                if (!neighbour.walkable || neighbour.isTower|| closedSet.Contains(neighbour))
                {
                    continue;
                }

                int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }
                }
            }
        }
    }
}
