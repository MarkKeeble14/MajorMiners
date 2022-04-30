using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using Utilities;

namespace Grid
{
    [CreateAssetMenu(fileName = "TileManager", menuName = "TileManager", order = 1)]
    public class TileManager : ScriptableObject
    {
        [SerializeField] private Tile blockedTile;
        [SerializeField] private Tile walkableTile;
        private Tilemap _tileMap;
        private Vector3 _stageDimensions;
        private TileState[,] _tileStates;

        public void SetWalkable(int row, int col)
        {
            _tileMap.SetTile(new Vector3Int(row, col, 1), walkableTile);
            Tile self = GetTile(row, col);
            AdjustCoordinates(ref row, ref col);
            _tileStates[row, col] = new Walkable(self);
        }

        public Tile GetTile(int row, int col)
        {
            return (Tile)_tileMap.GetTile(new Vector3Int(row, col, 1));
        }

        public Vector3 GetTileWorldPos(int row, int col)
        {
            var t = GetTile(row, col).transform;
            return _tileMap.GetCellCenterWorld(new Vector3Int((int)t.m30, (int)t.m31, (int)t.m32));
        }
        
        public void SetTileStates(Tilemap tileMap)
        {
            _tileMap = tileMap;
            _stageDimensions = _tileMap.size;
            Initialize();
        }

        private void Initialize()
        {
            _tileStates = new TileState[(int)_stageDimensions.x, (int)_stageDimensions.y];
            for (var i = 0; i < _tileStates.GetLength(0); ++i)
            {
                for (var j = 0; j < _tileStates.GetLength(1); ++j)
                {
                    int row = i;
                    int col = j;
                    DeadjustCoordinates(ref row, ref col);
                    _tileStates[i, j] = new Blocked(GetTile(row, col));
                }
            }
            Helper.PrintMatrix(_tileStates);
        }

        private void AdjustCoordinates(ref int row, ref int col)
        {
            row += (int)_stageDimensions.x / 2;
            col += (int)_stageDimensions.y / 2;
        }

        private void DeadjustCoordinates(ref int row, ref int col)
        {
            row -= (int)_stageDimensions.x / 2;
            col -= (int)_stageDimensions.y / 2;
        }
    }
}