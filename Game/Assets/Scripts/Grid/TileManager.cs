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

        [SerializeField] private int spaceAroundAsteroidHorizontal;
        [SerializeField] private int spaceAroundAsteroidVertical;

        [SerializeField] private int spaceAroundCanyonHorizontal;
        [SerializeField] private int spaceAroundCanyonVertical;

        [SerializeField] private GameObject worldTile;
        [SerializeField] private GameObject asteroidTile;
        private WorldTile[,] _tiles;

        private void OnDisable()
        {
            rows -= spaceAroundCanyonVertical;
            cols -= spaceAroundCanyonHorizontal;
        }

        public void SpawnGrid(Transform parent)
        {
            rows += spaceAroundCanyonVertical;
            cols += spaceAroundCanyonHorizontal;
            Vector3 startPos = new Vector3(-rows / 2, cols / 2, 0);

            _tiles = new WorldTile[rows, cols];
            for (int i = 0; i < rows; ++i)
            {
                for (int j = 0; j < cols; ++j)
                {
                    Vector3 spawnPos = startPos + new Vector3(i, -j, 0);
                    GameObject spawned;
                    if (spawnPos == Vector3.zero)   // Middle
                    {
                        // spawn resource
                        spawned = Instantiate(asteroidTile, spawnPos, Quaternion.identity);
                    }
                    else if (   // Space Around Middle
                            spawnPos.x > -spaceAroundAsteroidHorizontal
                        && spawnPos.x < spaceAroundAsteroidHorizontal
                        && spawnPos.y > -spaceAroundAsteroidVertical
                        && spawnPos.y < spaceAroundAsteroidVertical)
                    {
                        spawned = Instantiate(worldTile, spawnPos, Quaternion.identity);
                        spawned.GetComponent<WorldTile>().SetBreakable(false);
                    }
                    else if (
                        spawnPos.x < -rows / 2 + spaceAroundCanyonHorizontal
                        || spawnPos.x > rows / 2 - spaceAroundCanyonHorizontal
                        || spawnPos.y < -cols / 2 + spaceAroundCanyonVertical
                        || spawnPos.y > cols / 2 - spaceAroundCanyonHorizontal
                        )
                    {
                        spawned = Instantiate(worldTile, spawnPos, Quaternion.identity);
                        spawned.GetComponent<WorldTile>().SetBreakable(false);
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