using UnityEngine;

public abstract class HealthEventSubscriber : MonoBehaviour
{
    private bool _isSubscribed = false;

    protected virtual void HandleDamage(int health) { }
    protected virtual void HandleHealing(int health) { }
    protected virtual void HandleDeath() { }
        
    private void OnEnable()
    {
        if (HealthManager.Instance != null && !_isSubscribed)
        {
            HealthManager.Instance.OnDamaged += HandleDamage;
            HealthManager.Instance.OnDeath += HandleDeath;
            HealthManager.Instance.OnHealed += HandleHealing;
            _isSubscribed = true;
        }
    }

    private void Start() // In case Instance was not ready during OnEnabled
    {
        if (HealthManager.Instance != null && !_isSubscribed)
        {
            HealthManager.Instance.OnDamaged += HandleDamage;
            HealthManager.Instance.OnDeath += HandleDeath;
            HealthManager.Instance.OnHealed += HandleHealing;
            _isSubscribed = true;
        }
    }

    private void OnDisable()
    {
        if (HealthManager.Instance != null && _isSubscribed)
        {
            HealthManager.Instance.OnDamaged -= HandleDamage;
            HealthManager.Instance.OnDeath -= HandleDeath;
            HealthManager.Instance.OnHealed -= HandleHealing;
            _isSubscribed = false;
        }
    }
}
