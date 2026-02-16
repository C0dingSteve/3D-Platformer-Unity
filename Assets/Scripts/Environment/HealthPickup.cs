using UnityEngine;
using Assets.Scripts.ServiceLocator;

public class HealthPickup : PickupItem
{
    [SerializeField] private int _healAmount;
    [SerializeField] private bool _isFullHeal;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            HealthManager healthManager = ServiceLocator.Get<HealthManager>();
            _healAmount = _isFullHeal ? healthManager.MaxHealth : _healAmount;
            healthManager.Heal(_healAmount);
            EndEffect();
        }
    }
}
