using UnityEngine;

public class BaseDefenderProjectile : MonoBehaviour
{
    [field: SerializeField] public float Speed { get; private set; }
    [field: SerializeField] public float BaseDamage { get; private set; } = 1.0f;
    [SerializeField] private float maxDistanceToTargetBeforeHit = 0.1f;

    public GameObject CurrentTarget { get; set; }
    private BaseProjectileEffect projectileEffect;
    private bool _hasHitTarget;

    private void Awake()
    {
        projectileEffect = GetComponent<BaseProjectileEffect>();
    }

    private void Update()
    {
        if (!_hasHitTarget)
        {
            UpdateMovement();
            _hasHitTarget = CheckIfHasHitTarget();
        }

        if (!_hasHitTarget) return;
        
        projectileEffect.UpdateEffect(CurrentTarget, BaseDamage);
        
        if (projectileEffect.IsDoneEffect)
        {
            Destroy(this);
        }
    }

    private void UpdateMovement()
    {
        Vector2.MoveTowards(transform.position, CurrentTarget.transform.position, Speed);
    }

    private bool CheckIfHasHitTarget()
    {
        return Vector2.Distance(transform.position, CurrentTarget.transform.position) <= maxDistanceToTargetBeforeHit;
    }
}