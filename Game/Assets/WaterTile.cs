using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTile : MonoBehaviour
{
    private SpriteRenderer sr;
    [SerializeField] private Sprite rightTile;
    [SerializeField] private Sprite leftTile;
    [SerializeField] private Sprite downTile;
    [SerializeField] private Sprite upTile;
    [SerializeField] private Sprite midTile;

    [SerializeField] private Sprite rightUpTile;
    [SerializeField] private Sprite rightDownTile;
    [SerializeField] private Sprite leftUpTile;
    [SerializeField] private Sprite leftDownTile;

    [SerializeField] private Sprite fullWaterTile;

    [SerializeField] private LayerMask land;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void RaycastSetSprite()
    {
        List<ArrowDirection> landDirectionsAdjacent = GetAdjacentBlocked();
        if (landDirectionsAdjacent.Count == 0)
        {
            List<ArrowDirection> landDirectionsDiagonal = GetDiagonalBlocked();
            SetSprite(landDirectionsDiagonal);
        }
        else
        {
            SetSprite(landDirectionsAdjacent);
        }
    }

    private List<ArrowDirection> GetAdjacentBlocked()
    {
        List<ArrowDirection> waterDirections = new List<ArrowDirection>();
        RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(-1, 0, 0), Vector3.forward,
            Mathf.Infinity, land);
        if (hit)
            waterDirections.Add(ArrowDirection.LEFT);

        hit = Physics2D.Raycast(transform.position + new Vector3(1, 0, 0), Vector3.forward, Mathf.Infinity, land);
        if (hit)
            waterDirections.Add(ArrowDirection.RIGHT);

        hit = Physics2D.Raycast(transform.position + new Vector3(0, 1, 0), Vector3.forward, Mathf.Infinity, land);
        if (hit)
            waterDirections.Add(ArrowDirection.UP);

        hit = Physics2D.Raycast(transform.position + new Vector3(0, -1, 0), Vector3.forward, Mathf.Infinity, land);
        if (hit)
            waterDirections.Add(ArrowDirection.DOWN);
        return waterDirections;
    }

    private List<ArrowDirection> GetDiagonalBlocked()
    {
        List<ArrowDirection> landDirections = new List<ArrowDirection>();

        RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(1, 1, 0),
            Vector3.forward, Mathf.Infinity, land);
        if (hit)
            landDirections.Add(ArrowDirection.RIGHTUP);

        hit = Physics2D.Raycast(transform.position + new Vector3(1, -1, 0),
            Vector3.forward, Mathf.Infinity, land);
        if (hit)
            landDirections.Add(ArrowDirection.RIGHTDOWN);

        hit = Physics2D.Raycast(transform.position + new Vector3(-1, 1, 0),
            Vector3.forward, Mathf.Infinity, land);
        if (hit)
            landDirections.Add(ArrowDirection.LEFTUP);

        hit = Physics2D.Raycast(transform.position + new Vector3(-1, -1, 0),
            Vector3.forward, Mathf.Infinity, land);
        if (hit)
            landDirections.Add(ArrowDirection.LEFTDOWN);

        return landDirections;
    }

    private void SetSprite(List<ArrowDirection> blocked)
    {
        Debug.Log(blocked.Count);
        if (blocked.Count == 1)
        {
            switch (blocked[0])
            {
                case ArrowDirection.UP:
                    sr.sprite = upTile;
                    break;
                case ArrowDirection.DOWN:
                    sr.sprite = downTile;
                    break;
                case ArrowDirection.RIGHT:
                    sr.sprite = rightTile;
                    break;
                case ArrowDirection.LEFT:
                    sr.sprite = leftTile;
                    break;
                case ArrowDirection.LEFTDOWN:
                    sr.sprite = leftDownTile;
                    break;
                case ArrowDirection.LEFTUP:
                    sr.sprite = leftUpTile;
                    break;
                case ArrowDirection.RIGHTDOWN:
                    sr.sprite = rightDownTile;
                    break;
                case ArrowDirection.RIGHTUP:
                    sr.sprite = rightUpTile;
                    break;
            }
        }
        else if (blocked.Count == 0)
        {
            sr.sprite = fullWaterTile;
        }
        else
        {
            sr.sprite = midTile;
        }
    }
}
