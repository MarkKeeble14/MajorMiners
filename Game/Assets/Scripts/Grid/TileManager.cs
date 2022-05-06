using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using Utilities;
using UnityEngine.SceneManagement;

public enum GenerationMethod
{
    BASE,
    DUMB_RANDOM,
    PERLIN_NOISE
}

namespace Grid
{
    [CreateAssetMenu(fileName = "TileManager", menuName = "TileManager", order = 1)]
    public class TileManager : ScriptableObject
    {
        [Header("Basic Generation Settings")]
        [SerializeField] private NumReadFrom rows;
        public int Rows
        {
            get { return (int)rows.Value; }
        }
        [SerializeField] private NumReadFrom cols;

        public int Columns
        {
            get { return (int)cols.Value; }
        }

        [SerializeField] private NumReadFrom aroundAsteroid;
        public int AroundAsteroid
        {
            get { return (int)aroundAsteroid.Value; }
        }
        [SerializeField] private NumReadFrom  aroundCanyon;
        public int AroundCanyon
        {
            get { return (int)aroundCanyon.Value; }
        }
        [SerializeField] private int waterAroundLand;
        [SerializeField] private GenerationMethod generationMethod = GenerationMethod.PERLIN_NOISE;

        [Header("Dumb Random Generation Settings")]
        // Out of 100
        [SerializeField] private int baseChanceToSpawnTile = 100;

        [Header("Perlin Noise Generation Settings")]
        /*
        [SerializeField, Range(0.01f, 0.99f)] private float noiseScale = 0.1f;
        [SerializeField, Range(0.01f, 0.99f)] private float noiseSeverity = 0.4f;
        */
        [SerializeField] private NumReadFrom noiseScale;
        public float NoiseScale
        {
            get { return noiseScale.Value; }
        }
        [SerializeField] private NumReadFrom noiseSeverity;
        public float NoiseSeverity
        {
            get { return noiseSeverity.Value; }
        }

        private float[,] _noiseMap;

        [Header("Sprites & Such")]
        [SerializeField] private LayerMask land;
        [SerializeField] private Sprite[] asteroidSpriteArray;
        [SerializeField] private Sprite aroundAsteroidSprite;

        [SerializeField] private GameObject worldTile;
        [SerializeField] private GameObject asteroidTile;
        [SerializeField] private GameObject waterTile;

        private Transform _parent;
        private WorldTile[,] _tiles;
        private List<WaterTile> waterTiles;

        private void GenerateNoise()
        {
            _noiseMap = new float[Rows, Columns];
            Vector2 offset = new Vector2(UnityEngine.Random.Range(-10000f, 10000f), UnityEngine.Random.Range(-10000f, 10000f));
            for (int i = 0; i < Rows; ++i)
            {
                for (int j = 0; j < Columns; ++j)
                {
                    float noise = Mathf.PerlinNoise(i * NoiseScale + offset.x, j * NoiseScale + offset.y);
                    _noiseMap[i, j] = noise;
                }
            }
        }

        private void DestroyGrid()
        {
            if (_tiles == null) return;
            foreach (Transform child in _parent.transform)
            {
                Destroy(child.gameObject);
            }
        }

        public void SpawnGrid(Transform parent)
        {
            DestroyGrid();
            _parent = parent;
            _tiles = new WorldTile[Rows, Columns];
            waterTiles = new List<WaterTile>();
            GenerateNoise();
            int asteroidTilesPlaced = 0;
            Vector3 startPos = new Vector3(-Rows / 2, Columns / 2, 0);
            for (int i = 0; i < Rows; ++i)
            {
                for (int j = 0; j < Columns; ++j)
                {
                    Vector3 spawnPos = startPos + new Vector3(i, -j, 1);
                    GameObject spawned;
                    if (i > Rows / 2 - 2 && i < 2 + Rows / 2 &&
                        j > Columns / 2 - 2 && j < 2 + Columns / 2)   // Middle
                    {
                        // Spawn resource
                        spawned = Instantiate(asteroidTile, spawnPos, Quaternion.identity);
                        WorldTile spawnedTile = spawned.GetComponent<WorldTile>();
                        spawnedTile.SetLockedSprite(asteroidSpriteArray[asteroidTilesPlaced++]);
                    }
                    else if (i > Rows / 2 - AroundAsteroid - 2 && i < AroundAsteroid + 2 + Rows / 2
                        && j > Columns / 2 - AroundAsteroid - 2 && j < AroundAsteroid + 2 + Columns / 2)
                    {
                        spawned = Instantiate(worldTile, spawnPos, Quaternion.identity);
                        WorldTile spawnedTile = spawned.GetComponent<WorldTile>();
                        spawnedTile.SetLockedSprite(aroundAsteroidSprite);
                        spawnedTile.SetBreakable(false);
                        spawnedTile.SetWalkable(true);
                    }
                    else if (i < AroundCanyon || i > Rows - AroundCanyon - 1
                        || j < AroundCanyon || j > Columns - AroundCanyon - 1)
                    {
                        spawned = Instantiate(worldTile, spawnPos, Quaternion.identity);
                        WorldTile spawnedTile = spawned.GetComponent<WorldTile>();
                        spawnedTile.SetBreakable(false);
                    }
                    else
                    {
                        if (generationMethod == GenerationMethod.DUMB_RANDOM)
                        {
                            if (RandomHelper.RandomIntInclusive(0, 100) <= baseChanceToSpawnTile)
                            {
                                spawned = Instantiate(worldTile, spawnPos, Quaternion.identity);
                                WorldTile spawnedTile = spawned.GetComponent<WorldTile>();
                                spawnedTile.SetRock();
                            }
                            else
                            {
                                // Spawn Regular tile instead
                                spawned = Instantiate(worldTile, spawnPos, Quaternion.identity);
                                WorldTile spawnedTile = spawned.GetComponent<WorldTile>();
                                spawnedTile.SetLockedSprite(aroundAsteroidSprite);
                                spawnedTile.SetBreakable(false);
                                spawnedTile.SetWalkable(true);
                            }
                        } else if (generationMethod == GenerationMethod.PERLIN_NOISE)
                        {
                            if (_noiseMap[i, j] > NoiseSeverity)
                            {
                                spawned = Instantiate(worldTile, spawnPos, Quaternion.identity);
                                WorldTile spawnedTile = spawned.GetComponent<WorldTile>();
                                spawnedTile.SetRock();
                            } else
                            {
                                spawned = Instantiate(worldTile, spawnPos, Quaternion.identity);
                                WorldTile spawnedTile = spawned.GetComponent<WorldTile>();
                                spawnedTile.SetLockedSprite(aroundAsteroidSprite);
                                spawnedTile.SetBreakable(false);
                                spawnedTile.SetWalkable(true);
                            }
                        } else
                        {
                            spawned = Instantiate(worldTile, spawnPos, Quaternion.identity);
                            WorldTile spawnedTile = spawned.GetComponent<WorldTile>();
                            spawnedTile.SetRock();

                        }
                    }
                    spawned.transform.SetParent(_parent);
                    _tiles[i, j] = spawned.GetComponent<WorldTile>();
                }
            }

            foreach (WorldTile t in _tiles)
            {
                if (!t.Breakable)
                    t.RaycastSetSprite();
            }
            SpawnWater();
        }

        private void SpawnWater()
        {
            Vector3 startPos = new Vector3(-Rows / 2 - waterAroundLand, Columns / 2 + waterAroundLand, 0);
            for (int i = 0; i < Rows + waterAroundLand * 2; i++)
            {
                for (int j = 0; j < Columns + waterAroundLand * 2; j++)
                {
                    Vector3 tilePos = startPos + new Vector3(i, -j, 0);
                    RaycastHit2D hit = Physics2D.Raycast(tilePos, Vector3.forward, Mathf.Infinity, land);

                    if (!hit)
                    {
                        GameObject spawnedWater = Instantiate(waterTile, tilePos, Quaternion.identity);
                        spawnedWater.transform.SetParent(_parent);
                        waterTiles.Add(spawnedWater.GetComponent<WaterTile>());
                    }
                }
            }

            foreach (WaterTile t in waterTiles)
            {
                t.RaycastSetSprite();
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