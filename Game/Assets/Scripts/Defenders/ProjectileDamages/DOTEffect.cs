using UnityEngine;
using UnityEngine.Tilemaps;

public class DOTEffect : BaseProjectileEffect
{
    [SerializeField] private int totalNumberOfDamageEffects = 3;
    private int _currentNumberOfDamageEffects;

    private float currentDealDamageTimer;
    [SerializeField] private float timePerDamageEffect = 0.5f;
    
    public override void UpdateEffect(GameObject effectTarget, float baseDamage)
    {
        if (currentDealDamageTimer > 0)
        {
            currentDealDamageTimer -= Time.deltaTime;
        }
        else
        {
            BaseAttacker hitAttacker = effectTarget.GetComponent<BaseAttacker>();
            hitAttacker.DealDamage(baseDamage);
            currentDealDamageTimer = timePerDamageEffect;

            if (++_currentNumberOfDamageEffects > totalNumberOfDamageEffects)
                IsDoneEffect = true;
        }
    }
}
