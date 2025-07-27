using UnityEngine;

public class CoinPickup : PickupItem
{
    [SerializeField] private int _amount = 5;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.AddMoney(_amount);
            EndEffect();
        }    
    }
}
