using Grid;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileCursor : MonoBehaviour
{
    [SerializeField] private TileManager tileManager;
    [SerializeField] private Tile currentTile;
    [SerializeField] public Vector2Int coordinates;
    private SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        SetCurrentTile();
    }

    private void SetCurrentTile()
    {
        currentTile = tileManager.GetTile(coordinates.x, coordinates.y);
        transform.position = currentTile.gameObject.transform.position;
    }

    public void Move(int rowWise, int colWise)
    {
        coordinates += new Vector2Int(rowWise, colWise);
        SetCurrentTile();
    }

    public void Show()
    {
        sr.enabled = true;
    }

    public void Hide()
    {
        sr.enabled = false;
    }
}