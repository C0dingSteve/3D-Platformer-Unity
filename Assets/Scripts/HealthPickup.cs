using UnityEngine;

public class HealthPickup : PickupItem
{
    [SerializeField] private int _healAmount;
    [SerializeField] private bool _isFullHeal;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _healAmount = _isFullHeal ? HealthManager.Instance.MaxHealth : _healAmount;
            other?.GetComponent<HealthManager>().Heal(_healAmount);

            EndEffect();
        }
    }
}
