using Grid;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitPathfind : MonoBehaviour
{
    [SerializeField] protected TileManager tileManager;
    [SerializeField] protected Transform seeker, target;
    protected MyGrid grid;
    protected UnitMove move;

    protected virtual void Awake()
    {
        grid = FindObjectOfType<MyGrid>();
        move = GetComponent<UnitMove>();
    }

    public abstract void FindTarget();

    public void FindNewPath()
    {
        grid.CreateGrid();
        move.Stop();
        FindTarget();
        FindPath();
    }

    public void FindPath()
    {
        FindPath(seeker.position, target.position);
    }

    public abstract void FindPath(Vector3 startPos, Vector3 targetPos);

    // Function to get the distance of two nodes
    protected int GetDistance(Node nodeA, Node nodeB)
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

    // Retraces and calculates what the path it took was, the parent node in the Node class is vital
    protected void RetracePath(Node startNode, Node endNode)
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
        move.GoToDest();
        // (FOR TESTING)
    }
}
