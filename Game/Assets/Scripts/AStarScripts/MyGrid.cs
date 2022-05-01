using System.Collections;
using System.Collections.Generic;
using Grid;
using UnityEngine;

//Class to represent the grid, the entire map
public class MyGrid : MonoBehaviour
{
    //  (defined in unity editor)
    public LayerMask unwalkableMask;
    //  (defined in unity editor)
    public LayerMask breakableMask;
    //  (defined in unity editor)
    public LayerMask towerMask;
    //Size the grid will cover (defined in unity editor)
    public TileManager _tileManager;

    public Vector2 gridWorldSize
    {
        get { return new Vector2(_tileManager.Rows, _tileManager.Columns); }
    }
    // How much space each node covers (defined in unity editor)
    public float nodeRadius;
    Node[,] grid;

    float nodeDiameter;
    // Amount of nodes fits into our grid world
    public int gridSizeX, gridSizeY;

    // Defines how many nodes we can fit into our grid world size
    void Update()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();

    }

    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        // WorldBottomLeft = (position (0, 0, 0)) - (left edge of the world) - (bottom left corner)
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.up * gridWorldSize.y / 2; // Vector3.forward gives the z axis in 3d space

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                // As x increases we go in increments of node diameter until we reach the edge (same thing for y, aka z axis for world space)
                // Each point a node will occupy
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.up * (y * nodeDiameter + nodeRadius);
                // Collision check (true if we don't collide with anything in the walkable mask)
                Vector2 box = new Vector2(nodeDiameter - 0.1f, nodeDiameter - 0.1f);
                bool walkable = !(Physics2D.OverlapBox(worldPoint, box, 90, unwalkableMask));
                bool breakable = (Physics2D.OverlapBox(worldPoint, box, 90, breakableMask));
                bool isTower = (Physics2D.OverlapBox(worldPoint, box, 90, towerMask));
                // Create a point on the grid using the Node class
                grid[x, y] = new Node(walkable, worldPoint, x, y);
                grid[x, y].row = y;
                grid[x, y].column = x;
                grid[x, y].breakable = breakable;
                grid[x, y].isTower = isTower;
            }
        }
    }

    // Gets a list of all the neighbouring nodes
    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();
        // Search by a 3x3 block around node
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                //Skip the current node
                if (x == 0 && y == 0 || Mathf.Abs(x) == Mathf.Abs(y)) // EDIT MADE IT SO IT DOESNT DO DIAGONALS
                {
                    continue;
                }
                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }
        return neighbours;
    }

    // Get the node from where the player is currently standing from the grid
    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        // Gets the position in percentage of the world size (middle would be 0.5, far left is 0, far right would be 1)
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        // NOTE: world position z because z in the world space is y in the grid space
        float percentY = (worldPosition.y + gridWorldSize.y / 2) / gridWorldSize.y;
        // Line will make sure it is not out of the world position (without might cause invalid index)
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);
        // Gets the index of the grid array (minus 1 because of index of array)
        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return grid[x, y];
    }



    // (FOR TESTING)
    public List<Node> path;
    void OnDrawGizmos()
    {
        // Lets us draw the gridworldsize in the unity editor
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y, 1));

        if (grid != null)
        {
            foreach (Node n in grid)
            {
                // if n is walkable then its white but if its collision then its red
                if (!n.walkable)
                {
                    Gizmos.color = Color.red;
                }
                if (n.breakable)
                {
                    Gizmos.color = Color.blue;
                }
                if (n.isTower)
                {
                    Gizmos.color = Color.green;
                }
                if (n.walkable && !n.breakable && !n.isTower)
                {
                    Gizmos.color = Color.white;
                }

                // Set the path color to black
                if (path != null)
                {
                    if (path.Contains(n))
                    {
                        Gizmos.color = Color.black;
                    }
                }

                // worldpos = center, vector3.one is (1,1,1) multiply by node diameter and subtract a bit of space for more outline
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - 0.1f));
            }
        }
    }
    // (FOR TESTING)
}
