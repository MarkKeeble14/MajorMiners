using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSeekerToTarget : MonoBehaviour
{
    //[SerializeField] private Grid.TileManager tileManager;

    public GameObject Astar;
    MyGrid grid;
    public List<Node> path;
    float speed = 10.0f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            grid = Astar.GetComponent<MyGrid>();
            path = grid.path;

            StartCoroutine(MoveToEachPosition());
        }
    }

    IEnumerator MoveToEachPosition()
    {

        for (int i = 0; i < path.Count; i++)
        {
            yield return MoveTo(path[i].worldPosition);//tileManager.GetTileWorldPos(path[i].row, path[i].column));//path[i].worldPosition);
        }
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
