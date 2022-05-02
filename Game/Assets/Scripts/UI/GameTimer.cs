using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GameTimer : MonoBehaviour
    {
        [SerializeField] private float gameTime;
        [SerializeField] private float hurryUpTime = 30;
        private TextMeshProUGUI _text;
        private void Start()
        {
            _text = transform.GetComponent<TextMeshProUGUI>();
            _text.text = Mathf.Round(gameTime).ToString(CultureInfo.CurrentUICulture);
        }

        // Update is called once per frame
        private void Update()
        {
            if (gameTime <= hurryUpTime)
            {
                GameManager.HurryUp();
            }
            if (gameTime <= 0) return;
            gameTime -= Time.deltaTime;
            _text.text = Mathf.Round(gameTime).ToString(CultureInfo.CurrentUICulture);
        }
    }
}
