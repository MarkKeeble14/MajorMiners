using FMODUnity;
using UnityEngine;

public class BaseAttacker : BaseUnit
{
    [field: SerializeField] public uint CurrencyDropped { get; private set; } = 100;
    [field: SerializeField] public float TotalHealth { get; private set; }
    [SerializeField] private GameObject bloodEffect;
    [SerializeField] private GameObject deathEffect;

    private float _currentHealth;

    public override void Awake()
    {
        base.Awake();
        
        _currentHealth = TotalHealth;
    }

    public void DealDamage(float damageDealt)
    {
        _currentHealth -= damageDealt;
        Instantiate(bloodEffect, transform);
        if (_currentHealth <= 0)
        {
            OnDeath();
        }
    }

    private void OnDeath()
    {
        RuntimeManager.PlayOneShot("event:/SFX/Human_Death", transform.position);
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
