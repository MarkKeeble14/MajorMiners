using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class GameTimerController : MonoBehaviour
    {
        [SerializeField] private TimerReadFrom timer;
        [SerializeField] private float hurryUpTime = 30;
        private TextMeshProUGUI _text;
        public float GameTime
        {
            get { return timer.Value; }
        }

        [SerializeField] private ResultsScreenController resultsScreen;

        private void Awake()
        {
            _text = transform.GetComponent<TextMeshProUGUI>();
            _text.text = Mathf.Round(GameTime).ToString(CultureInfo.CurrentUICulture);
        }

        // Update is called once per frame
        private void Update()
        {
            if (GameTime <= hurryUpTime)
            {
                GameManager.HurryUp();
            }
            if (GameTime <= 0)
            {
                resultsScreen.DefenderWon(VICTORY_METHOD.TIMED_OUT);
            }
            else {
                timer.Subtract(Time.deltaTime);
                _text.text = Mathf.Round(GameTime).ToString(CultureInfo.CurrentUICulture);
            }
        }
    }
}
