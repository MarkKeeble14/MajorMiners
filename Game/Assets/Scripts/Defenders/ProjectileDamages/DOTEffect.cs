using UnityEngine;
using UnityEngine.Tilemaps;

public class DOTEffect : BaseProjectileEffect
{
    [SerializeField] private ushort totalNumberOfDamageEffects = 3;
    [SerializeField] private float timePerDamageEffect = 0.5f;
    
    private Timer _dealDamageTimer;
    private ushort _currentNumberOfDamageEffects;

    private void Awake()
    {
        _dealDamageTimer = new Timer(timePerDamageEffect);
    }
    
    public override void UpdateEffect(GameObject effectTarget, float baseDamage)
    {
        if (!_dealDamageTimer.IsFinished())
        {
            _dealDamageTimer.UpdateTime();
        }

        if (!_dealDamageTimer.IsFinished()) return;
        _dealDamageTimer.Reset();
        
        // Deal damage to enemy.

        ++_currentNumberOfDamageEffects;

        if (_currentNumberOfDamageEffects < totalNumberOfDamageEffects) return;
        
        IsDoneEffect = true;
    }
}
