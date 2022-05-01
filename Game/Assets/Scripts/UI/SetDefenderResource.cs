using TMPro;
using UnityEngine;

namespace UI
{
    public class SetDefenderResource : MonoBehaviour
    {
        [SerializeField] private GameManager gameManager;

        // Start is called before the first frame update
        private TextMeshProUGUI _text;
        private Player _defender;

        private void Start()
        {
            _text = GetComponent<TextMeshProUGUI>();
            _defender = gameManager.defender;
            _text.text = _defender.money.ToString();
        }

        // Update is called once per frame
        private void FixedUpdate()
        {
            _text.text = _defender.money.ToString();
        }
    }
}