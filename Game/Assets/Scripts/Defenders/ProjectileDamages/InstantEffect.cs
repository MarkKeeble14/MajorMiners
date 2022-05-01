using UnityEngine;

public class InstantEffect : BaseProjectileEffect
{
    public override void UpdateEffect(GameObject effectTarget, float baseDamage)
    {
        var hitAttacker = effectTarget.GetComponent<BaseAttacker>();
        
        hitAttacker.DealDamage(baseDamage);
    }
}
