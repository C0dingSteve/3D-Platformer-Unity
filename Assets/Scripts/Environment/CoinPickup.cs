using UnityEngine;
using Assets.Scripts.ServiceLocator;

public class CoinPickup : PickupItem
{
    [SerializeField] private int _amount = 5;

    private GameManager _gameManager;
    private GameHUD _gameHUD;

    private void Awake()
    {
        _gameManager = ServiceLocator.Get<GameManager>();
        _gameHUD = ServiceLocator.Get<GameHUD>();  
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _gameManager.AddMoney(_amount);
            _gameHUD.UpdateMoney(_amount);
            EndEffect();
        }    
    }
}