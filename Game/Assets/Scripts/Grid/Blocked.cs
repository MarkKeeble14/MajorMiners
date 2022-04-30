using UnityEngine.Tilemaps;
using UnityEngine;

namespace Grid
{
    public class Blocked : TileState
    {
        public Blocked(Tile t)
        {
            Self = t;
        }
    }
}