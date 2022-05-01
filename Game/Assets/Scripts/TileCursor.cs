using Grid;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileCursor : MonoBehaviour
{
    [SerializeField] public TileManager tileManager;
    [SerializeField] public WorldTile currentTile;
    [SerializeField] public Vector2Int coordinates;
    private SpriteRenderer sr;
    public bool Enabled
    {
        get { return sr.enabled; }
    }

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        SetTo(0, 0);
    }

    private void SetCurrentTile()
    {
        currentTile = tileManager.GetTile(coordinates.x, coordinates.y);
        transform.position = currentTile.transform.position;
    }

    public void SetTo(int row, int col)
    {
        coordinates = new Vector2Int(row, col);
        SetCurrentTile();
    }

    public void Move(int rowWise, int colWise)
    {
        if (coordinates.x + rowWise > tileManager.Rows - 1
            || coordinates.x + rowWise < 0)
            return;
        if (coordinates.y + colWise > tileManager.Columns - 1
            || coordinates.y + colWise < 0)
            return;
        Vector2Int moveBy = new Vector2Int(rowWise, colWise);
        coordinates += moveBy;
        SetCurrentTile();
    }

    public void SetBreakable(bool breakable)
    {
        if (!Enabled)
            return;
        currentTile.SetBreakable(breakable);
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