using System;
using UnityEngine;

public class BaseUnit : MonoBehaviour
{
    [field: SerializeField] public uint Cost { get; private set; } = 100;
    [field: SerializeField] public uint Damage { get; private set; } = 1000;

    public static int numberOfUnits;

    public float placeCD = 0.2f;

    public virtual void Awake()
    {
        GameManager.IncreaseUnits();
    }

    protected virtual void OnDestroy()
    {
        GameManager.DecreaseUnits();
    }
}