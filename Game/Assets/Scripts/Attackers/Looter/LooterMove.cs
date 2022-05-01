using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LooterMove : MonoBehaviour
{
    MyGrid grid;
    public List<Node> path;

    [SerializeField] private float moveSpeed = 1.0f;
    [SerializeField] private GameObject resourceEffect;
    bool onRoute = false;

    private void Awake()
    {
        grid = FindObjectOfType<MyGrid>();
    }

    // Update is called once per frame
    public void GoToDest()
    {
        if (!onRoute)
        {
            path = grid.path;

            StartCoroutine(MoveToEachPosition());
            onRoute = true;
        }
    }


    IEnumerator MoveToEachPosition()
    {

        for (int i = 0; i < path.Count; i++)
        {
            yield return MoveTo(path[i].worldPosition);
        }
        Instantiate(resourceEffect, transform.position, Quaternion.identity);
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
