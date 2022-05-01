using System.Collections;
using System.Collections.Generic;
using Grid;
using UnityEngine;

public class MinerMove : MonoBehaviour
{
    public Grid.TileManager tileManager;

    public GameObject Astar;
    MyGrid grid;
    public List<Node> path;

    [SerializeField] float moveSpeed = 5.0f;
    [SerializeField] float mineSpeed = 1.0f;
    [SerializeField] private GameObject dirtEffect;

    bool onRoute = false;

    private void Awake()
    {
        Astar = FindObjectOfType<MyGrid>().gameObject;
    }

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
                yield return new WaitForSeconds(mineSpeed);
                WorldTile current = tileManager.GetTile(path[i].gridX, grid.gridSizeY - path[i].gridY - 1);
                Instantiate(dirtEffect, current.transform);
                current.SetBreakable(false);
            }
            yield return MoveTo(path[i].worldPosition);
        }
        onRoute = false;
        FindObjectOfType<AttackerPlayer>().money += GetComponent<BaseUnit>().Cost;
        Destroy(gameObject);
    }

    IEnumerator MoveTo(Vector3 destination)
    {
        while (transform.position != destination)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
