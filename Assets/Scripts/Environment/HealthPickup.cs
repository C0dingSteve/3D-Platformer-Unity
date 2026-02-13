using UnityEngine;
using Assets.Scripts.ServiceLocator;

public class HealthPickup : PickupItem
{
    [SerializeField] private int _healAmount;
    [SerializeField] private bool _isFullHeal;

    private HealthManager _healthManager;

    private void Awake()
    {
        _healthManager = ServiceLocator.Get<HealthManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _healAmount = _isFullHeal ? _healthManager.MaxHealth : _healAmount;
            _healthManager.Heal(_healAmount);
            EndEffect();
        }
    }
}
