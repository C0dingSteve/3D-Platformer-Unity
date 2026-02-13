using UnityEngine;
using Assets.Scripts.ServiceLocator;

public class CoinPickup : PickupItem
{
    [SerializeField] private int _amount = 5;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ServiceLocator.Get<GameManager>().AddMoney(_amount);
            ServiceLocator.Get<UIManager>().UpdateMoney(_amount);
            EndEffect();
        }    
    }
}
