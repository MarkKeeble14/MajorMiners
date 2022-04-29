using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TimerBarController : MonoBehaviour
{
    private BetterBarDisplay bar;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private TimerBarReadFrom readFrom;

    private void Start()
    {
        bar = GetComponent<BetterBarDisplay>();
    }

    public void SetSize(float value, float max)
    {
        if (value >= max || max == 0)
            bar.Enable = false;
        else
            bar.Enable = true;
        if (max == 0)
            return;
        bar.SetSize(value, max); 
        text.text = readFrom.Subject + ": " + Math.Round(max - value, 1);
    }

    private void Update()
    {
        SetSize(readFrom.Timer, readFrom.Duration);
    }
}
