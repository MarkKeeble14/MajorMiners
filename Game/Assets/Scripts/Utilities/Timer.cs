using System;
using UnityEngine;

[Serializable]
public class Timer
{
    [field: SerializeField] public float TotalTime { get; private set; }
    public float ElapsedTime { get; private set; }

    public Timer(float totalTime)
    {
        TotalTime = totalTime;
    }

    public void UpdateTime()
    {
        if (IsFinished()) return;
        
        ElapsedTime += Time.deltaTime;
    }

    public bool IsFinished()
    {
        return ElapsedTime >= TotalTime;
    }

    public void SetTime(float time)
    {
        ElapsedTime = time;
    }

    public void Reset()
    {
        if (IsFinished())
        {
            ElapsedTime -= TotalTime;
        }
        else
        {
            ElapsedTime = 0;
        }
    }
}
