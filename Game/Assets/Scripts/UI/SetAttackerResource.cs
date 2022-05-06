using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UI
{
    public class SetAttackerResource : MonoBehaviour
    {
        [SerializeField] private GameManager gameManager;
        // Start is called before the first frame update
        private TextMeshProUGUI _text;
        private Player _attacker;
        private void Start()
        {
            _text = GetComponent<TextMeshProUGUI>();
            _attacker = gameManager.attacker;
            _text.text = _attacker.Money.ToString();
        }

        // Update is called once per frame
        private void FixedUpdate()
        {
            _text.text = _attacker.Money.ToString();
        }
    }
}
