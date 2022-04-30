using UnityEngine;

public class BaseDefenderProjectile : MonoBehaviour
{
    [field: SerializeField] public float Speed { get; private set; }

    public GameObject CurrentTarget { get; set; }

    private void Update()
    {
        UpdateMovement();
    }

    private void UpdateMovement()
    {
        Vector2.MoveTowards(transform.position, CurrentTarget.transform.position, Speed);
    }

    private void HitTarget()
    {
        // TODO: Deal damage to enemy.
    }
}
