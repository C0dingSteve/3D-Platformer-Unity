using TMPro;
using UnityEngine;
using Assets.Scripts.ServiceLocator;

public class GameHUD: MonoBehaviour, IGameHUD
{
    // UI References
    [SerializeField] private TMP_Text _healthText;
    [SerializeField] private TMP_Text _moneyText;

    private HealthManager _healthManager;
    private GameManager _gameManager;

    private void Awake()
    {
        ServiceLocator.Register(this);

        _healthManager = ServiceLocator.Get<HealthManager>();
        _gameManager = ServiceLocator.Get<GameManager>();

        gameObject.AddComponent<HealthHUDBridge>();
        
        UpdateGameHUD(_healthManager.MaxHealth, _gameManager.Money);
    }
    
    public void UpdateGameHUD(int health, int money)
    {
        UpdateHealth(health);
        UpdateMoney(money);
    }

    public void UpdateHealth(int health) => _healthText.text = $"{health}/{_healthManager.MaxHealth}";
    public void UpdateMoney(int amount) => _moneyText.text = amount.ToString();
}