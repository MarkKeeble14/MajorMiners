using UnityEngine;
using UnityEngine.UI;

public class RadialBarDisplay : MonoBehaviour
{
    [SerializeField] private Image radialIndicatorUI;

    public void SetSize(float value, float max)
    {
        radialIndicatorUI.fillAmount = value / max;
    }
}
