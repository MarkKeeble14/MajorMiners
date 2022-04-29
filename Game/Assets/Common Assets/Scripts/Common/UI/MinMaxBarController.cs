using UnityEngine;
using TMPro;

public class MinMaxBarController : MonoBehaviour
{
    private BetterBarDisplay bar;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private MinMaxBarReadFrom readFrom;

    private void Start()
    {
        bar = GetComponent<BetterBarDisplay>();
    }

    public void SetSize(float value, float max)
    {
        if (max == 0)
            return;
        bar.SetSize(value, max);
        text.text = readFrom.Subject + ": " + System.Math.Round(value, 2) + " / " + System.Math.Round(max, 2);
    }

    private void Update()
    {
        SetSize(readFrom.Value, readFrom.Max);
    }
}
