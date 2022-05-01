using UnityEngine;

public class BaseDefenderProjectile : MonoBehaviour
{
    [field: SerializeField] public float Speed { get; private set; } = 0.5f;
    [field: SerializeField] public float BaseDamage { get; private set; } = 1.0f;
    [SerializeField] private float maxDistanceToTargetBeforeHit = 0.1f;
    
    public GameObject CurrentTarget { get; set; }
    private bool _hasHitTarget;
    private BaseProjectileEffect _projectileEffect;

    private void Awake()
    {
        _projectileEffect = GetComponent<BaseProjectileEffect>();
    }

    private void Update()
    {
        if (!_hasHitTarget)
        {
            UpdateMovement();
            _hasHitTarget = CheckIfHasHitTarget();
        }

        if (!_hasHitTarget) return;
        
        _projectileEffect.UpdateEffect(CurrentTarget, BaseDamage);
        
        if (_projectileEffect.IsDoneEffect)
        {
            Destroy(gameObject);
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