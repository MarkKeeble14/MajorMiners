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
            Debug.Log($"row: {row}, col: {col}");
            _tileStates[row, col] = new Walkable(self);
        }

        public Tile GetTile(int row, int col)
        {
            return null;
            // return _tileStates[row, col].Tile();
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
                    _tileStates[i, j] = new Blocked();
                }
            }
            Helper.PrintMatrix(_tileStates);
        }

        private void AdjustCoordinates(ref int row, ref int col)
        {
            row += (int)_stageDimensions.x / 2;
            col += (int)_stageDimensions.y / 2;
        }
    }
}