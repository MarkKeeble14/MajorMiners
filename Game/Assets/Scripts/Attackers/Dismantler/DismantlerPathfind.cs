using System.Collections.Generic;
using Grid;
using UnityEngine;

public class DismantlerPathfind : MonoBehaviour
{
    [SerializeField] private TileManager tileManager;
    public DismantlerMove dismantlerMove;
    public Transform seeker, target;
    public MyGrid grid;
    void Update()
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
        if (target != null)
        {
            FindPath(seeker.position, target.position);
        }
    }


    // Gets the grid from the MyGrid Script
    void Awake()
    {
        grid = FindObjectOfType<MyGrid>();
    }

    // Refer to the PSEUDO-CODE in A* algorithm notes
    void FindPath(Vector3 startPos, Vector3 targetPos)
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

    // Retraces and calculates what the path it took was, the parent node in the Node class is vital
    void RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        // Path will be backwards, need to reverse
        path.Reverse();

        // (FOR TESTING)
        grid.path = path;
        dismantlerMove.GoToDest();
        // (FOR TESTING)
    }

    // Function to get the distance of two nodes
    int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        // 14 is the unit for diagonals and 10 is non-diagonals, this is a fancy math equation refer to notes on A* algorithm
        if (dstX > dstY)
        {
            return 14 * dstY + 10 * (dstX - dstY);
        }
        return 14 * dstX + 10 * (dstY - dstX);
    }


}
