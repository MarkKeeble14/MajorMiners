using UnityEngine;

public class BaseDefenderShoot : MonoBehaviour
{
    [SerializeField] private float shootDelay;

    private Timer _shootTimer;

    private void Awake()
    {
        _shootTimer = new Timer(shootDelay);
    }

    public void UpdateShoot(GameObject currentTarget)
    {
        if (!_shootTimer.IsFinished())
        {
            _shootTimer.UpdateTime();
        }
        else
        {
            ShootTarget(currentTarget);
            _shootTimer.Reset();
        }
    }

    protected virtual void ShootTarget(GameObject currentTarget)
    {
        // TODO: Shoot projectile.
    }
}
