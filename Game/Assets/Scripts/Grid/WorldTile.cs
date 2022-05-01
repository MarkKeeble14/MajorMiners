using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;
using System.Collections.Generic;

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

        public void SetAsteroid(bool asteroid)
        {
            sr.sprite = walkableSprite;
            gameObject.layer = LayerMask.NameToLayer(walkableLayer);
        }
    }
}