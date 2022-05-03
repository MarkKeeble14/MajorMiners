using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace Grid
{
    public class WorldTile : MonoBehaviour
    {
        public List<GameObject> ActiveMiners = new List<GameObject>();
        public bool Breakable;
        public bool Walkable;
        [SerializeField] private Sprite[] breakableSprite;
        [SerializeField] private Sprite walkableSprite;
        [SerializeField] private Sprite towerSprite;
        private SpriteRenderer sr;
        private string breakableLayer = "Breakable";
        private string walkableLayer = "Walkable";
        private string towerLayer = "Tower";
        public GameObject occupyingTower;

        public bool spriteLocked;

        [SerializeField] private Sprite rightTile;
        [SerializeField] private Sprite leftTile;
        [SerializeField] private Sprite downTile;
        [SerializeField] private Sprite upTile;
        [SerializeField] private Sprite midTile;

        [SerializeField] private Sprite rightUpTile;
        [SerializeField] private Sprite rightDownTile;
        [SerializeField] private Sprite leftUpTile;
        [SerializeField] private Sprite leftDownTile;

        [SerializeField] private Sprite grassTile;
        [SerializeField] private LayerMask block;

        [SerializeField] private Vector2 minMaxResourceValue;
        public uint resourceValue { get; private set; }

        private void Awake()
        {
            sr = GetComponent<SpriteRenderer>();
            resourceValue = (uint)RandomHelper.RandomIntInclusive(minMaxResourceValue);
        }

        public void RaycastSetSprite()
        {
            if (spriteLocked)
                return;
            List<ArrowDirection> blockedDirectionsAdjacent = GetAdjacentBlocked();
            if (blockedDirectionsAdjacent.Count == 0)
            {
                List<ArrowDirection> blockedDirectionsDiagonal = GetDiagonalBlocked();
                SetSprite(blockedDirectionsDiagonal);
            }
            else
            {
                SetSprite(blockedDirectionsAdjacent);
            }
        }

        private List<ArrowDirection> GetAdjacentBlocked()
        {
            List<ArrowDirection> blockedDirections = new List<ArrowDirection>();
            RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(-1, 0, 0), Vector3.forward,
                Mathf.Infinity, block);
            if (hit)
                blockedDirections.Add(ArrowDirection.LEFT);

            hit = Physics2D.Raycast(transform.position + new Vector3(1, 0, 0), Vector3.forward, Mathf.Infinity, block);
            if (hit)
                blockedDirections.Add(ArrowDirection.RIGHT);

            hit = Physics2D.Raycast(transform.position + new Vector3(0, 1, 0), Vector3.forward, Mathf.Infinity, block);
            if (hit)
                blockedDirections.Add(ArrowDirection.UP);

            hit = Physics2D.Raycast(transform.position + new Vector3(0, -1, 0), Vector3.forward, Mathf.Infinity, block);
            if (hit)
                blockedDirections.Add(ArrowDirection.DOWN);
            return blockedDirections;
        }

        private List<ArrowDirection> GetDiagonalBlocked()
        {
            List<ArrowDirection> blockedDirections = new List<ArrowDirection>();

            RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(1, 1, 0),
                Vector3.forward, Mathf.Infinity, block);
            if (hit)
                blockedDirections.Add(ArrowDirection.RIGHTUP);

            hit = Physics2D.Raycast(transform.position + new Vector3(1, -1, 0),
    Vector3.forward, Mathf.Infinity, block);
            if (hit)
                blockedDirections.Add(ArrowDirection.RIGHTDOWN);

            hit = Physics2D.Raycast(transform.position + new Vector3(-1, 1, 0),
    Vector3.forward, Mathf.Infinity, block);
            if (hit)
                blockedDirections.Add(ArrowDirection.LEFTUP);

            hit = Physics2D.Raycast(transform.position + new Vector3(-1, -1, 0),
    Vector3.forward, Mathf.Infinity, block);
            if (hit)
                blockedDirections.Add(ArrowDirection.LEFTDOWN);

            return blockedDirections;
        }

        private void SetSprite(List<ArrowDirection> blocked)
        {
            if (spriteLocked) return;

            if (blocked.Count == 1)
            {
                switch (blocked[0])
                {
                    case ArrowDirection.UP:
                        sr.sprite = downTile;
                        break;
                    case ArrowDirection.DOWN:
                        sr.sprite = upTile;
                        break;
                    case ArrowDirection.RIGHT:
                        sr.sprite = leftTile;
                        break;
                    case ArrowDirection.LEFT:
                        sr.sprite = rightTile;
                        break;
                    case ArrowDirection.LEFTDOWN:
                        sr.sprite = leftDownTile;
                        break;
                    case ArrowDirection.LEFTUP:
                        sr.sprite = leftUpTile;
                        break;
                    case ArrowDirection.RIGHTDOWN:
                        sr.sprite = rightDownTile;
                        break;
                    case ArrowDirection.RIGHTUP:
                        sr.sprite = rightUpTile;
                        break;
                }
            }
            else if (blocked.Count == 0)
            {
                sr.sprite = grassTile;
            }
            else
            {
                sr.sprite = midTile;
            }
        }

        public void SetLockedSprite(Sprite sprite)
        {
            sr.sprite = sprite;
            spriteLocked = true;
        }

        public void SetBreakable(bool breakable)
        {
            Breakable = breakable;
            SetTileData();
        }

        public void SetWalkable(bool walkable)
        {
            Walkable = walkable;
            SetTileData();
        }

        public void BreakBreakable()
        {
            SetBreakable(false);
            SetWalkable(true);
        }

        public void SetRock()
        {
            SetBreakable(true);
            SetWalkable(false);
        }

        private void SetTileData()
        {
            if (Walkable)
            {
                if (!spriteLocked) sr.sprite = walkableSprite;
                gameObject.layer = LayerMask.NameToLayer(walkableLayer);
            } else if (Breakable)
            {
                if (!spriteLocked) sr.sprite = breakableSprite[RandomHelper.RandomIntInclusive(0, breakableSprite.Length - 1)];
                gameObject.layer = LayerMask.NameToLayer(breakableLayer);
            } else if (occupyingTower != null)
            {
                if (!spriteLocked) sr.sprite = towerSprite;
                gameObject.layer = LayerMask.NameToLayer(towerLayer);
            }
        }

        public void SetTower(GameObject tower)
        {
            if (!tower)
            {
                Destroy(occupyingTower);
            }
            occupyingTower = tower;
            Breakable = false;
            Walkable = false;
            SetTileData();
        }
    }
}