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
        [SerializeField] private int rows;
        public int Rows
        {
            get { return rows; }
        }
        [SerializeField] private int cols;

        public int Columns
        {
            get { return cols; }
        }

        [SerializeField] private int aroundAsteroid;
        [SerializeField] private int aroundCanyon;

        [SerializeField] private Sprite outsideCanyonSprite;
        [SerializeField] private Sprite[] asteroidSpriteArray;

        [SerializeField] private GameObject worldTile;
        [SerializeField] private GameObject asteroidTile;

        private WorldTile[,] _tiles;

        public void SpawnGrid(Transform parent)
        {
            Vector3 startPos = new Vector3(-rows / 2, cols / 2, 0);

            int asteroidTilesPlaced = 0;
            _tiles = new WorldTile[rows, cols];
            for (int i = 0; i < rows; ++i)
            {
                for (int j = 0; j < cols; ++j)
                {
                    Vector3 spawnPos = startPos + new Vector3(i, -j, 1);
                    GameObject spawned;
                    if (i > rows / 2 - 1 || i < rows / 2 + 1 ||
                        j > cols / 2 - 1 || j < cols / 2 + 1)   // Middle
                    {
                        // Spawn resource
                        spawned = Instantiate(asteroidTile, spawnPos, Quaternion.identity);
                        spawned.GetComponent<SpriteRenderer>().sprite
                            = asteroidSpriteArray[asteroidTilesPlaced++];

                    }
                    else if (i > rows / 2 - aroundAsteroid - 1 && i < aroundAsteroid + rows / 2 + 1
                        && j > cols / 2 - aroundAsteroid - 1 && j < aroundAsteroid + cols / 2 + 1)
                    {
                        spawned = Instantiate(worldTile, spawnPos, Quaternion.identity);
                        spawned.GetComponent<WorldTile>().SetBreakable(false);
                    }
                    else if (i < aroundCanyon || i > rows - aroundCanyon - 1
                        || j < aroundCanyon || j > cols - aroundCanyon - 1)
                    {
                        spawned = Instantiate(worldTile, spawnPos, Quaternion.identity);
                        spawned.GetComponent<WorldTile>().SetBreakable(false);
                        spawned.GetComponent<SpriteRenderer>().sprite = outsideCanyonSprite;
                    }
                    else
                    {
                        spawned = Instantiate(worldTile, spawnPos, Quaternion.identity);
                        spawned.GetComponent<WorldTile>().SetBreakable(true);
                    }
                    spawned.transform.SetParent(parent);
                    _tiles[i, j] = spawned.GetComponent<WorldTile>();
                }
            }

        }

        public void SetWalkable(int row, int col, bool breakable)
        {
            WorldTile tile = _tiles[row, -col];
            tile.SetBreakable(breakable);
        }

        public WorldTile GetTile(int row, int col)
        {
            return _tiles[row, col];
        }
    }
}