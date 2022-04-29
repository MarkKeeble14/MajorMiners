using UnityEngine;

public class RadialMinMaxBarController : MonoBehaviour
{
    private RadialBarDisplay bar;
    [SerializeField] private MinMaxBarReadFrom readFrom;

    public void SetReadFrom(MinMaxBarReadFrom rf)
    {
        readFrom = rf;
    }

    private void Start()
    {
        bar = GetComponent<RadialBarDisplay>();
    }

    public void SetSize(float value, float max)
    {
        if (max == 0)
            return;
        bar.SetSize(value, max);
    }

    private void Update()
    {
        SetSize(readFrom.Value, readFrom.Max);
    }
}
