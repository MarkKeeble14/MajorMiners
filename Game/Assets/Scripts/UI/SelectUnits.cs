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
            units[0].GetComponent<Image>().sprite = disable;
            units[1].GetComponent<Image>().sprite = disable;
            units[2].GetComponent<Image>().sprite = disable;
        }

        
        /// <summary>
        /// Get a reference to this script and call these methods on an input to switch.
        /// We can separate inputs from functionality this way since attacker and defender have different controls.
        /// </summary>
        
        public void SelectFirst()
        {
            units[0].GetComponent<Image>().sprite = enable;
            units[1].GetComponent<Image>().sprite = disable;
            units[2].GetComponent<Image>().sprite = disable;
        }

        public void SelectSecond()
        {
            units[0].GetComponent<Image>().sprite = disable;
            units[1].GetComponent<Image>().sprite = enable;
            units[2].GetComponent<Image>().sprite = disable;
        }

        public void SelectThird()
        {
            units[0].GetComponent<Image>().sprite = disable;
            units[1].GetComponent<Image>().sprite = disable;
            units[2].GetComponent<Image>().sprite = enable;
        }
    }
}
