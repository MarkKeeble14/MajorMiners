using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace UI
{
    public class SelectUnits : MonoBehaviour
    {
        // Start is called before the first frame update
        [SerializeField] private List<GameObject> units;
        [SerializeField] private Sprite disable;
        [SerializeField] private Sprite enable;

        private void Start()
        {
            units[0].GetComponent<Image>().sprite = enable;
            units[0].GetComponent<Image>().transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            units[1].GetComponent<Image>().sprite = disable;
            units[2].GetComponent<Image>().sprite = disable;
        }

        
        /// <summary>
        /// Get a reference to this script and call these methods on an input to switch.
        /// We can separate inputs from functionality this way since attacker and defender have different controls.
        /// </summary>
        
        public void SelectUnit(int index)
        {
            switch (index)
            {
                case 0:
                    units[0].GetComponent<Image>().sprite = enable;
                    units[0].GetComponent<Image>().transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                    units[1].GetComponent<Image>().sprite = disable;
                    units[1].GetComponent<Image>().transform.localScale = new Vector3(1f, 1f,1f);
                    units[2].GetComponent<Image>().sprite = disable;
                    units[2].GetComponent<Image>().transform.localScale = new Vector3(1f, 1f, 1f);
                    break;
                case 1:
                    units[0].GetComponent<Image>().sprite = disable;
                    units[0].GetComponent<Image>().transform.localScale = new Vector3(1f, 1f, 1f);
                    units[1].GetComponent<Image>().sprite = enable;
                    units[1].GetComponent<Image>().transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                    units[2].GetComponent<Image>().sprite = disable;
                    units[2].GetComponent<Image>().transform.localScale = new Vector3(1f, 1f, 1f);
                    break;
                case 2:
                    units[0].GetComponent<Image>().sprite = disable;
                    units[0].GetComponent<Image>().transform.localScale = new Vector3(1f, 1f, 1f);
                    units[1].GetComponent<Image>().sprite = disable;
                    units[1].GetComponent<Image>().transform.localScale = new Vector3(1f, 1f, 1f);
                    units[2].GetComponent<Image>().sprite = enable;
                    units[2].GetComponent<Image>().transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                    break;
                default:
                    units[0].GetComponent<Image>().sprite = disable;
                    units[0].GetComponent<Image>().transform.localScale = new Vector3(1f, 1f, 1f);
                    units[1].GetComponent<Image>().sprite = disable;
                    units[1].GetComponent<Image>().transform.localScale = new Vector3(1f, 1f, 1f);
                    units[2].GetComponent<Image>().sprite = disable;
                    units[2].GetComponent<Image>().transform.localScale = new Vector3(1f, 1f, 1f);
                    break;
            }
        }
    }
}
