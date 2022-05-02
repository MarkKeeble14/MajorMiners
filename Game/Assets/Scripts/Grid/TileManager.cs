using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using Utilities;
using UnityEngine.SceneManagement;

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
        [SerializeField] private int waterAroundLand;

        [SerializeField] private Sprite outsideCanyonSprite;
        [SerializeField] private Sprite[] asteroidSpriteArray;

        [SerializeField] private GameObject worldTile;
        [SerializeField] private GameObject asteroidTile;
        [SerializeField] private GameObject waterTile;

        [SerializeField] private LayerMask land;

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
                    if (i > rows / 2 - 2 && i < 2 + rows / 2 &&
                        j > cols / 2 - 2 && j < 2 + cols / 2)   // Middle
                    {
                        // Spawn resource
                        spawned = Instantiate(asteroidTile, spawnPos, Quaternion.identity);
                        WorldTile spawnedTile = spawned.GetComponent<WorldTile>();
                        spawnedTile.SetLockedSprite(asteroidSpriteArray[asteroidTilesPlaced++]);
                    }
                    else if (i > rows / 2 - aroundAsteroid - 2 && i < aroundAsteroid + 2 + rows / 2
                        && j > cols / 2 - aroundAsteroid - 2 && j < aroundAsteroid + 2 + cols / 2)
                    {
                        spawned = Instantiate(worldTile, spawnPos, Quaternion.identity);
                        WorldTile spawnedTile = spawned.GetComponent<WorldTile>();
                        spawnedTile.SetBreakable(false);
                        spawnedTile.spriteLocked = true;
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
                        WorldTile spawnedTile = spawned.GetComponent<WorldTile>();
                        spawnedTile.SetBreakable(true);
                        spawnedTile.spriteLocked = true;
                    }
                    spawned.transform.SetParent(parent);
                    _tiles[i, j] = spawned.GetComponent<WorldTile>();
                }
            }

            foreach (WorldTile t in _tiles)
            {
                t.RaycastSetSprite();
            }
            // SpawnWater();
        }

        private void SpawnWater()
        {
            Vector3 startPos = new Vector3(-rows / 2 - waterAroundLand, cols / 2 + waterAroundLand, 0);
            for (int i = 0; i < rows + 2 * waterAroundLand; i++)
            {
                for (int j = 0; j < cols + 2 * waterAroundLand; j++)
                {
                    Vector3 tilePos = startPos + new Vector3(i, j, 0);
                    RaycastHit2D hit = Physics2D.Raycast(tilePos,
                    Vector3.forward, Mathf.Infinity, land);
                    if (!hit)
                    {
                        GameObject spawnedWater = Instantiate(waterTile, tilePos, Quaternion.identity);
                    }
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