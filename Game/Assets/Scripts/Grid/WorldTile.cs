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
        public bool Walkable = false;
        [SerializeField] private Sprite walkableSprite;
        [SerializeField] private Sprite blockableSprite;
        private SpriteRenderer sr;

        private void Awake()
        {
            sr = GetComponent<SpriteRenderer>();
            SetWalkable(false);
        }

        public void SetWalkable(bool walkable)
        {
            Walkable = walkable;
            sr.sprite = Walkable ? walkableSprite : blockableSprite;
        }
        
    }
}