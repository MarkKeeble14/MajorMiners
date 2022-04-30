using UnityEngine;

public abstract class BaseProjectileEffect : MonoBehaviour
{
    public bool IsDoneEffect { get; protected set; }

    public abstract void UpdateEffect(GameObject effectTarget, float baseDamage);
}
