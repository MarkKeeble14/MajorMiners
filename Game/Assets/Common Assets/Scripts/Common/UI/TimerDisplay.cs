using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerDisplay : MonoBehaviour
{
    [SerializeField] private NumReadFrom timer;

    // Update is called once per frame
    void Update()
    {
        timer.Value += Time.deltaTime;
    }
}
