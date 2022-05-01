using UnityEngine;

public class BaseUnit : MonoBehaviour
{
    [field: SerializeField] public uint Cost { get; private set; } = 100;
    
}
