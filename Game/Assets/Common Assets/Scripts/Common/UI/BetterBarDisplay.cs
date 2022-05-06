using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterBarDisplay : MonoBehaviour
{
    [SerializeField] private RectTransform bar;
    private bool enable = true;
    public bool Enable
    {
        get { return enable; }
        set { enable = value; }
    }
    private CanvasGroup grp;

    private void Start()
    {
        grp = GetComponent<CanvasGroup>();
    }

    public void SetSize(float value, float max)
    {
        float num = value / max;
        if (num < 0)
        {
            num = 0;
        }
        bar.localScale = new Vector3(num, 1f);
    }

    private void Update()
    {
        if (Enable)
        {
            grp.alpha = 1;
        } else
        {
            grp.alpha = 0;
        }
    }
}
