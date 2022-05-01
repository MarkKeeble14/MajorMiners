using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;
using System.Collections.Generic;

public enum ArrowDirection
{
    LEFT,
    RIGHT,
    UP,
    DOWN,

    LEFTUP,
    RIGHTUP,
    LEFTDOWN,
    RIGHTDOWN
}

namespace Grid
{
    public class WorldTile : MonoBehaviour
    {
        public List<GameObject> ActiveMiners = new List<GameObject>();
        public bool Breakable;
        [SerializeField] private Sprite breakableSprite;
        [SerializeField] private Sprite walkableSprite;
        [SerializeField] private Sprite towerSprite;
        private SpriteRenderer sr;
        private string breakableLayer = "Breakable";
        private string walkableLayer = "Walkable";
        private string towerLayer = "Tower";
        private GameObject occupyingTower;

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

        private void Awake()
        {
            sr = GetComponent<SpriteRenderer>();
        }

        public void SetBreakable(bool breakable)
        {
            Breakable = breakable;
            sr.sprite = Breakable ? breakableSprite : walkableSprite;
            gameObject.layer = LayerMask.NameToLayer(Breakable ? breakableLayer : walkableLayer);
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
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, Mathf.Infinity, block);
            if (hit)
                blockedDirections.Add(ArrowDirection.LEFT);

            hit = Physics2D.Raycast(transform.position, Vector2.right, Mathf.Infinity, block);
            if (hit)
                blockedDirections.Add(ArrowDirection.RIGHT);

            hit = Physics2D.Raycast(transform.position, Vector2.up, Mathf.Infinity, block);
            if (hit)
                blockedDirections.Add(ArrowDirection.UP);

            hit = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity, block);
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
        public void SetTower(GameObject tower)
        {
            if (!tower)
            {
                Destroy(occupyingTower);
            }
            occupyingTower = tower;
            Breakable = false;
            sr.sprite = tower ? towerSprite : walkableSprite;
            gameObject.layer = LayerMask.NameToLayer(tower ? towerLayer : walkableLayer);
        }
    }
}