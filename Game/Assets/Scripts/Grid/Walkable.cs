using UnityEngine;
using UnityEngine.Tilemaps;

namespace Grid
{
    public class Walkable : TileState
    {
        public Walkable(Tile t)
        {
            Self = t;
        }
    }
}