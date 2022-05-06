using System;
using System.Collections;
using System.Collections.Generic;
using Grid;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridTileManager : MonoBehaviour
{
    [SerializeField] private TileManager tileManager;
    [SerializeField] private TileCursor[] cursors;

    [SerializeField] private bool spawnOnStart;
    [SerializeField] private bool setCursors;

    private void Start()
    {
        if (spawnOnStart)
            Spawn();
    }

    public void Spawn()
    {
        tileManager.SpawnGrid(transform);
        if (!setCursors) return;
        foreach (TileCursor cursor in cursors)
        {
            cursor.SnapToGrid();
        }
    }
}
