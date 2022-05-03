using FMODUnity;
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
    [SerializeField] private Vector2Int startAt;
    [SerializeField] private bool invert;
    [SerializeField] private bool startInMiddle;
    private SpriteRenderer sr;
    [SerializeField] private bool alienCursor;
    public bool Enabled
    {
        get { return sr.enabled; }
    }

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        if (startInMiddle)
        {
            SetTo(tileManager.Rows / 2, tileManager.Columns / 2);
        } else
        {
            if (invert)
            {
                SetTo(tileManager.Rows - startAt.x - 1, tileManager.Columns - startAt.y - 1);
            }
            else
            {
                SetTo(startAt.x, startAt.y);
            }
        }
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
        PlayCursorSoundFX();
        Vector2Int moveBy = new Vector2Int(rowWise, colWise);
        coordinates += moveBy;
        SetCurrentTile();
    }


    private void PlayCursorSoundFX()
    {
        if (alienCursor)
        {
            RuntimeManager.PlayOneShot("event:/SFX/Alien_Cursor", currentTile.transform.position);
        }
        else
        {
            RuntimeManager.PlayOneShot("event:/SFX/Human_Cursor", currentTile.transform.position);
        }
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