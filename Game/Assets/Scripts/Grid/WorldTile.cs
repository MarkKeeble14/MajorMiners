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
        [SerializeField] private Sprite unwalkableSprite;
        private SpriteRenderer sr;
        private string breakableLayer = "Breakable";
        private string unwalkableLayer = "Unwalkable";

        private void Awake()
        {
            sr = GetComponent<SpriteRenderer>();
        }

        public void SetBreakable(bool breakable)
        {
            Breakable = breakable;
            sr.sprite = Breakable ? breakableSprite : unwalkableSprite;
            gameObject.layer = LayerMask.NameToLayer(Breakable ? breakableLayer : unwalkableLayer);
        }
    }
}