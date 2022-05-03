using System.Collections.Generic;
using Grid;
using UnityEngine;

public class DismantlerPathfind : UnitPathfind
{
    void Update()
    {
        FindTarget();
        if (target != null)
        {
            FindPath(seeker.position, target.position);
        }
    }

    public override void FindTarget()
    {
        var foundDefenderObjects = GameObject.FindGameObjectsWithTag("Defender");
        float closestDistance = float.MaxValue;
        foreach (GameObject defender in foundDefenderObjects)
        {
            float dist = Vector3.Distance(defender.transform.position, transform.position);
            if (dist < closestDistance)
            {
                closestDistance = dist;
                target = defender.transform;
            }
        }
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
                if (neighbour.isTower)
                {
                    targetNode = neighbour;
                }
                if (!neighbour.walkable || closedSet.Contains(neighbour) || neighbour.breakable)
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
