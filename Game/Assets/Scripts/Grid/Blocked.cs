using UnityEngine.Tilemaps;

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