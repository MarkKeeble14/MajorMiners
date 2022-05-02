using System.Collections;
using System.Collections.Generic;
using System.IO;
using FMODUnity;
using UnityEngine;

public class DismantlerMove : MonoBehaviour
{
    public Grid.TileManager tileManager;

    public GameObject Astar;
    MyGrid grid;
    public List<Node> path;

    [SerializeField] float moveSpeed = 5.0f;
    [SerializeField] float mineSpeed = 1.0f;
    [SerializeField] private GameObject resourceEffect;

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
            if (path[i].isTower)
            {
                yield return new WaitForSeconds(mineSpeed);
                tileManager.GetTile(path[i].gridX, grid.gridSizeY - path[i].gridY - 1).SetTower(null);
                tileManager.GetTile(path[i].gridX, grid.gridSizeY - path[i].gridY - 1).SetBreakable(false);
                
                
                onRoute = false;
                FindObjectOfType<AttackerPlayer>().money += GetComponent<BaseUnit>().Cost;
                Instantiate(resourceEffect, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
            yield return MoveTo(path[i].worldPosition);
        }

        if (tileManager.GetTile(path[path.Count - 1].gridX, grid.gridSizeY - path[path.Count - 1].gridY - 1).occupyingTower)
        {
            FindObjectOfType<AttackerPlayer>().money += GetComponent<BaseUnit>().Cost;
        }


        onRoute = false;
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
