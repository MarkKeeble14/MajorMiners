using UnityEngine;

public class BaseAttacker : BaseUnit
{
    [field: SerializeField] public uint CurrencyDropped { get; private set; } = 100;
    [field: SerializeField] public float TotalHealth { get; private set; }

    private float _currentHealth;

    public void Awake()
    {
        _currentHealth = TotalHealth;
    }

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
