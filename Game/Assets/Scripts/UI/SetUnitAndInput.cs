using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SetUnitAndInput : MonoBehaviour
    {
        [SerializeField] private Sprite unit;

        [SerializeField] private char input;
        [SerializeField] private Image img;
        [SerializeField] private TextMeshProUGUI text;
        
        // Start is called before the first frame update
        void Start()
        {
            img.GetComponent<Image>().sprite = unit;
            text.GetComponent<TextMeshProUGUI>().text = input.ToString();
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
