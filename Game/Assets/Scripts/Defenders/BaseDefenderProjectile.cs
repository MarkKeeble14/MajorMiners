using UnityEngine;

public class BaseDefenderProjectile : MonoBehaviour
{
    [field: SerializeField] public float Speed { get; private set; } = 10.0f;
    [field: SerializeField] public float BaseDamage { get; private set; } = 1.0f;
    [SerializeField] private float maxDistanceToTargetBeforeHit = 0.1f;
    
    public GameObject CurrentTarget { get; set; }
    private bool _hasHitTarget;
    private BaseProjectileEffect _projectileEffect;
    private GameObject _numberPopup;

    private void Awake()
    {
        _projectileEffect = GetComponent<BaseProjectileEffect>();
        _numberPopup = (GameObject) Resources.Load("PopupText/PopupText");
    }

    private void Update()
    {
        if (!CurrentTarget)
        {
            Destroy(gameObject);
            return;
        }
        
        if (!_hasHitTarget)
        {
            UpdateMovement();
            _hasHitTarget = CheckIfHasHitTarget();
        }

        if (!_hasHitTarget) return;

        GetComponent<SpriteRenderer>().enabled = false;
        GameObject spawned = Instantiate(_numberPopup, CurrentTarget.transform.position, Quaternion.identity);
        spawned.GetComponent<PopupText>().Set(BaseDamage.ToString(), Color.black);
        _projectileEffect.UpdateEffect(CurrentTarget, BaseDamage);
        
        if (_projectileEffect.IsDoneEffect)
        {
            Destroy(gameObject);
        }
    }

    private void UpdateMovement()
    {
        transform.position = Vector2.MoveTowards(transform.position, CurrentTarget.transform.position, Speed * Time.deltaTime);
    }

    private bool CheckIfHasHitTarget()
    {
        return Vector2.Distance(transform.position, CurrentTarget.transform.position) <= maxDistanceToTargetBeforeHit;
    }
}