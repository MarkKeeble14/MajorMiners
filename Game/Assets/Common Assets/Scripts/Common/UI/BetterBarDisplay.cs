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
        bar.localScale = new Vector3(value / max, 1f);
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
