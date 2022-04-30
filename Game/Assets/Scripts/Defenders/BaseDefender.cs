using UnityEngine;

public class BaseDefender : MonoBehaviour
{
    [field: SerializeField] public float Cost { get; private set; }
    [field: SerializeField] public float BaseDamage { get; private set; } = 1.0f;
    [field: SerializeField] public float AggroRadius { get; private set; } = 5.0f;
    // The distance to lose aggro should always be larger than the aggro radius.
    [field: SerializeField] public float LoseAggroDistance { get; private set; } = 6.0f;
    [field: SerializeField] public LayerMask TargetLayerMask { get; private set; }

    [SerializeField] private float timeBetweenTargetingChecks = 5.0f;
    private Timer _targetingCheckTimer;
    private bool _isTargetingEnemy;
    private GameObject _currentTarget;

    private void Awake()
    {
        _targetingCheckTimer = new Timer(timeBetweenTargetingChecks);
    }

    private void Update()
    {
        UpdateTargetChecks();
    }

    /*
     * Every so often, updates the current target for this defender.
     * If not targeting, checks for units to target.
     * Else, sees if the currently targeted unit is still in range.
     */
    private void UpdateTargetChecks()
    {
        if (!_targetingCheckTimer.IsFinished())
        {
            UpdateTargetedEnemy();
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
            if (Vector2.Distance(transform.position, _currentTarget.transform.position) <= LoseAggroDistance) return;

            _isTargetingEnemy = false;
        }

        if (_isTargetingEnemy) return;
        
        // Try finding an enemy within range.
        var hitResult = Physics2D.CircleCast(transform.position, AggroRadius, Vector2.one, 0.0f, TargetLayerMask);
        var hitCollider = hitResult.collider;

        if (!hitCollider) return;
        
        _currentTarget = hitCollider.gameObject;
        _isTargetingEnemy = true;
    }
}