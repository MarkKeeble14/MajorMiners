using FMODUnity;
using UnityEngine;

public class BaseAttacker : BaseUnit
{
    [field: SerializeField] public uint CurrencyDropped { get; private set; } = 100;
    [field: SerializeField] public float TotalHealth { get; private set; }
    [SerializeField] private GameObject bloodEffect;
    [SerializeField] private GameObject deathEffect;

    private float _currentHealth;

    public delegate void OnDeath();
    public OnDeath onDeath;

    private BetterBarDisplay hpBar;
    private GameObject _numberPopup;

    public override void Awake()
    {
        base.Awake();
        _numberPopup = (GameObject)Resources.Load("PopupText/NumberPopupCanvas");
        hpBar = transform.GetChild(0).GetChild(0).GetComponent<BetterBarDisplay>();
        onDeath += Die;
        _currentHealth = TotalHealth;
    }

    public void DealDamage(float damageDealt)
    {
        // Deal Damage
        _currentHealth -= damageDealt;

        // Spawn Particles
        Instantiate(bloodEffect, transform.position, Quaternion.identity);

        // Play Hurt Sounds
        RuntimeManager.PlayOneShot("event:/SFX/Hit_Hurt");

        // Spawn Damage Number
        GameObject spawned = Instantiate(_numberPopup,
            transform.position,
            Quaternion.identity);
        GameObject numberPopup = spawned.transform.GetChild(0).gameObject;
        numberPopup.GetComponent<PopupText>().Set(damageDealt.ToString(), Color.black);

        // Update HP Bar
        hpBar.SetSize(_currentHealth, TotalHealth);

        // Die if health is depleted
        if (_currentHealth <= 0)
        {
            onDeath();
        }
    }

    private void Die()
    {
        RuntimeManager.PlayOneShot("event:/SFX/Human_Death", transform.position);
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        FindObjectOfType<DefenderPlayer>().AlterMoney(Mathf.RoundToInt(Cost / 5), transform.position);
        Destroy(gameObject);
    }
}
