using System;
using UnityEngine;
using Utilities;

namespace Grid
{
    public class TileManager : ScriptableObject
    {
        [SerializeField]
        public int stageDimensions;
        private TileState[,] _tileStates;

        private void OnEnable()
        {
            _tileStates = new TileState[stageDimensions, stageDimensions];
            for (var i = 0; i < stageDimensions; ++i)
                for (var j = 0; j < stageDimensions; ++j)
                    _tileStates[i, j] = TileState.Blocked;
        }

        public void SetWalkable(int row, int col)
        {
            if (_tileStates[row, col] == TileState.Water) return;
            _tileStates[row, col] = TileState.Walkable;
        }
    }
}
