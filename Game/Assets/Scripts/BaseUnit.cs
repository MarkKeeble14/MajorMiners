using UnityEngine;

public class BaseUnit : MonoBehaviour
{
    [field: SerializeField] public uint Cost { get; private set; } = 100;
    [field: SerializeField] public uint Damage { get; private set; } = 1000;

}
