using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinerMove : MonoBehaviour
{
    public Grid.TileManager tileManager;

    public GameObject Astar;
    MyGrid grid;
    public List<Node> path;

    float speed = 10.0f;
    bool onRoute = false;

    // Update is called once per frame
    public void GoToDest()
    {
        if (!onRoute)
        {
            grid = Astar.GetComponent<MyGrid>();
            path = grid.path;

            StartCoroutine(MoveToEachPosition());
            onRoute = true;
        }
    }


    IEnumerator MoveToEachPosition()
    {

        for (int i = 0; i < path.Count; i++)
        {
            if (path[i].breakable)
            {
                yield return new WaitForSeconds(0.5f);
                tileManager.GetTile(path[i].gridX, grid.gridSizeY - path[i].gridY - 1).SetBreakable(false);
            }
            yield return MoveTo(path[i].worldPosition);
        }
        onRoute = false;
    }

    IEnumerator MoveTo(Vector3 destination)
    {
        while (transform.position != destination)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
            yield return null;
        }
    }
}
