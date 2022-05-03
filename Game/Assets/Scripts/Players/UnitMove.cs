using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitMove : MonoBehaviour
{
    private GameObject gridGameObject;
    protected MyGrid grid;
    protected List<Node> path;
    protected bool onRoute = false;
    [SerializeField] protected float moveSpeed = 1.0f;
    [SerializeField] protected GameObject resourceEffect;
    protected BaseAttacker bAttacker;
    protected UnitPathfind uPathfind;

    protected void Awake()
    {
        gridGameObject = FindObjectOfType<MyGrid>().gameObject;
        grid = gridGameObject.GetComponent<MyGrid>();
        bAttacker = GetComponent<BaseAttacker>();
        uPathfind = GetComponent<UnitPathfind>();
    }

    public void Stop()
    {
        path = new List<Node>();
        StopAllCoroutines();
        onRoute = false;
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

    protected IEnumerator MoveTo(Vector3 destination)
    {
        while (transform.position != destination)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }

    protected abstract IEnumerator MoveToEachPosition();
}
