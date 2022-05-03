using TMPro;
using UnityEngine;

namespace UI
{
    public class SetPlayerResource : MonoBehaviour
    {
        [SerializeField] private Player player;

        // Start is called before the first frame update
        private TextMeshProUGUI _text;

        private void Start()
        {
            _text = GetComponent<TextMeshProUGUI>();
        }

        // Update is called once per frame
        private void Update()
        {
            _text.text = "$" + player.money.ToString();
        }
    }
}