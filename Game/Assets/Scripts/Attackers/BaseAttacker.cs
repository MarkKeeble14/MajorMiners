using UnityEngine;

public class BaseAttacker : MonoBehaviour
{
    [field: SerializeField] public float TotalHealth { get; private set; }

    private float _currentHealth = 1.0f;

    public void DealDamage(float damageDealt)
    {
        _currentHealth -= damageDealt;

        if (_currentHealth <= 0)
        {
            OnDeath();
        }
    }

    private void OnDeath()
    {
        Destroy(gameObject);
    }
}
