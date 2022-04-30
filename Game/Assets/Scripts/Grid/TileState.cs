using UnityEngine;
using UnityEngine.Tilemaps;

namespace Grid
{
    public class TileState
    {
        public Vector3 Position { get { return Self.gameObject.transform.position; } }
        public GameObject Miner { get; set; }
        public Tile Self { get; set; }
    }
}