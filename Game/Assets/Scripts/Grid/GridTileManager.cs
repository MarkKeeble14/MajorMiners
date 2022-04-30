using System;
using System.Collections;
using System.Collections.Generic;
using Grid;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridTileManager : MonoBehaviour
{
    [SerializeField] private TileManager tileManager;

    private void Awake()
    {
        tileManager.SpawnGrid(transform);
    }
}
