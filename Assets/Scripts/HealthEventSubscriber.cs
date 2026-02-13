using Assets.Scripts.ServiceLocator;
using UnityEngine;

public abstract class HealthEventSubscriber : MonoBehaviour
{
    private bool _isSubscribed = false;

    protected virtual void HandleDamage(int health) { }
    protected virtual void HandleHealing(int health) { }
    protected virtual void HandleDeath() { }
    
    private void OnEnable()
    {
        if (_isSubscribed) return;

        HealthManager _healthManager = ServiceLocator.Get<HealthManager>();
        _healthManager.OnDamaged += HandleDamage;
        _healthManager.OnDeath += HandleDeath;
        _healthManager.OnHealed += HandleHealing;
        _isSubscribed = true;
    }

    private void OnDisable()
    {
        if (!_isSubscribed) return;

        HealthManager _healthManager = ServiceLocator.Get<HealthManager>();
        _healthManager.OnDamaged -= HandleDamage;
        _healthManager.OnDeath -= HandleDeath;
        _healthManager.OnHealed -= HandleHealing;
        _isSubscribed = false;
    }
}
