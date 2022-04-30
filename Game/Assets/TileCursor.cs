using Grid;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileCursor : MonoBehaviour
{
    [SerializeField] private TileManager tileManager;
    [SerializeField] private TileState currentTileState;
    [SerializeField] public Vector2Int coordinates;
    private SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        SetCurrentTile();
    }

    private void SetCurrentTile()
    {
        currentTileState = tileManager.GetTileState(coordinates.x, coordinates.y);
    }

    public void Move(int rowWise, int colWise)
    {
        if (coordinates.x + rowWise > tileManager.StageDimensions.x - 1
            || coordinates.x + rowWise < 0)
            return;
        if (coordinates.y + colWise > tileManager.StageDimensions.y - 1
            || coordinates.y + colWise < 0)
            return;
        Vector2Int moveBy = new Vector2Int(rowWise, colWise);
        coordinates += moveBy;
        transform.position += new Vector3(moveBy.x, moveBy.y, 0);
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