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
        // public List<GameObject> ActiveMiners = new List<GameObject>();
        private SpriteRenderer sr;
        private Player attacker;
        public bool Walkable;
        public bool Breakable;
        private string breakableLayer = "Breakable";
        private string walkableLayer = "Walkable";
        private string towerLayer = "Tower";
        [SerializeField] private LayerMask block;
        public GameObject occupyingTower;

        public bool spriteLocked;

        [SerializeField] private Sprite[] breakableSprite;
        [SerializeField] private Sprite walkableSprite;
        [SerializeField] private Sprite towerSprite;
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

        [SerializeField] private Vector2 minMaxResourceValue;
        private int resourceValue;
        public int ResourceValue 
        { 
            get { return resourceValue; } 
            set { resourceValue = value; } 
        }
        [SerializeField] private GameObject mineBarCanvas;
        [SerializeField] private BetterBarDisplay mineBar;

        private bool designatedToMine;
        public bool DesignatedToMine
        {
            get { return designatedToMine; }
            set { designatedToMine = value; }
        }

        [SerializeField] private float baseTimeToMine;
        [SerializeField] private float timeToMineResourceFactor;
        private Timer mineTimer;
        private List<MinerMove> activeMiners = new List<MinerMove>();
        public void AddActiveMiner(MinerMove miner)
        {
            if (!Breakable) return;
            activeMiners.Add(miner);
            if (mineTimer == null)
            {
                mineTimer = new Timer(baseTimeToMine + (timeToMineResourceFactor * Mathf.Sqrt(resourceValue * 2) / 2));
                StartCoroutine(Mine());
            }
        }
        public void RemoveActiveMiner(MinerMove miner)
        {
            activeMiners.Remove(miner);
            if (activeMiners.Count <= 0)
            {
                mineBarCanvas.SetActive(false);
                StopAllCoroutines();
                mineTimer = null;
            }
        }
        public bool IsMined
        {
            get { return mineTimer.IsFinished(); }
        }

        private IEnumerator Mine()
        {
            mineBarCanvas.SetActive(true);
            while (!mineTimer.IsFinished())
            {
                for (int i = 0; i < activeMiners.Count; i++)
                {
                    mineTimer.UpdateTime();
                    mineBar.SetSize(mineTimer.ElapsedTime, mineTimer.TotalTime);
                }
                yield return null;
            }
            mineBarCanvas.SetActive(false);
        }

        private void Awake()
        {
            sr = GetComponent<SpriteRenderer>();
        }

        public void SetColor()
        {
            if (designatedToMine)
            {
                sr.color = new Color(1 - MathHelper.Normalize(resourceValue, minMaxResourceValue.x, minMaxResourceValue.y, .25f, 0),
                1 - MathHelper.Normalize(resourceValue, minMaxResourceValue.x, minMaxResourceValue.y, .5f, .25f),
                0, 1);
            }
            else
            {
                sr.color = new Color(1 - MathHelper.Normalize(resourceValue, minMaxResourceValue.x, minMaxResourceValue.y, .25f, 0),
                1 - MathHelper.Normalize(resourceValue, minMaxResourceValue.x, minMaxResourceValue.y, .25f, 0),
                0, 1);
            }
        }

        public void SetResourceValue()
        {
            resourceValue = RandomHelper.RandomIntInclusive(minMaxResourceValue);
            SetColor();
        }

        private void UnsetResourceValue()
        {
            resourceValue = 0;
            sr.color = new Color(1, 1, 1, 1);
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
            AttackerPlayer attacker = FindObjectOfType<AttackerPlayer>();
            if (attacker) attacker.AlterMoney(resourceValue, transform.position);
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
                UnsetResourceValue();
            } else if (Breakable)
            {
                if (!spriteLocked) sr.sprite = breakableSprite[RandomHelper.RandomIntInclusive(0, breakableSprite.Length - 1)];
                gameObject.layer = LayerMask.NameToLayer(breakableLayer);
                SetResourceValue();
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