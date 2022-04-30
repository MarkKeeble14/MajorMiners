using System;
using System.Collections;
using System.Collections.Generic;
using Grid;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridTileManager : MonoBehaviour
{
    [SerializeField] private TileManager tileManager;

    [SerializeField] private Tilemap tileMap;

    private void Awake()
    {
        tileManager.SetTileStates(tileMap);
    }
}
