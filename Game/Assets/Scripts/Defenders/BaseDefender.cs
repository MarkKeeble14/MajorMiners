using UnityEngine;

public class BaseDefender : MonoBehaviour
{
    [field: SerializeField] public float Cost { get; private set; } = 1.0f;
    [SerializeField] private float aggroRadius = 5.0f;
    // The distance to lose aggro should always be larger than the aggro radius.
    [SerializeField] private float loseAggroDistance = 6.0f;
    [SerializeField] private LayerMask targetLayerMask;
    [SerializeField] private float timeBetweenTargetingChecks = 1.0f;
    
    private BaseDefenderShoot baseDefenderShoot;
    private Timer _targetingCheckTimer;
    private bool _isTargetingEnemy;
    private GameObject _currentTarget;

    private void Awake()
    {
        _targetingCheckTimer = new Timer(timeBetweenTargetingChecks);
        baseDefenderShoot = GetComponent<BaseDefenderShoot>();
    }

    private void Update()
    {
        UpdateTargetChecks();
        UpdateShooting();
    }

    /*
     * Every so often, updates the current target for this defender.
     * If not targeting, checks for enemies to target.
     * Else, sees if the currently targeted enemy is still in range.
     */
    private void UpdateTargetChecks()
    {
        if (!_targetingCheckTimer.IsFinished())
        {
            UpdateTargetedEnemy();
            _targetingCheckTimer.Reset();
        }
        else
        {
            _targetingCheckTimer.UpdateTime();
        }
    }

    /**
     * If not targeting, checks for enemies within range.
     * Else, checks if the targeted enemy is still in range.
     */
    private void UpdateTargetedEnemy()
    {
        if (_isTargetingEnemy)
        {
            // See if targeted enemy is still in range.
            if (Vector2.Distance(transform.position, _currentTarget.transform.position) <= loseAggroDistance) return;

            _isTargetingEnemy = false;
        }

        if (_isTargetingEnemy) return;
        
        // Try finding an enemy within range.
        var hitResult = Physics2D.CircleCast(transform.position, aggroRadius, Vector2.one, 0.0f, targetLayerMask);
        var hitCollider = hitResult.collider;

        if (!hitCollider) return;
        
        _currentTarget = hitCollider.gameObject;
        _isTargetingEnemy = true;
    }

    private void UpdateShooting()
    {
        if (!_isTargetingEnemy) return;

        baseDefenderShoot.UpdateShoot(_currentTarget);
    }
}