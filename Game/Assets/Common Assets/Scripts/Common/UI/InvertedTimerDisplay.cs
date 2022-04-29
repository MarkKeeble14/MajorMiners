using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvertedTimerDisplay : MonoBehaviour
{
    [SerializeField] private NumReadFrom timer;

    // Update is called once per frame
    void Update()
    {
        if (timer.Value > 0)
            timer.Value -= Time.deltaTime;
        else
            timer.Value = 0;
    }
}
