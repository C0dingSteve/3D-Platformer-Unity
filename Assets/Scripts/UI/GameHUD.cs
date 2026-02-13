using TMPro;
using UnityEngine;
using Assets.Scripts.ServiceLocator;

public class GameHUD: MonoBehaviour, IGameHUD
{
    // UI References
    [SerializeField] private TMP_Text _healthText;
    [SerializeField] private TMP_Text _moneyText;

    private HealthManager _healthManager;

    public void Initialize()
    {
        gameObject.AddComponent<HealthHUDBridge>();
        _healthManager = ServiceLocator.Get<HealthManager>();
    }
    
    public void UpdateHealth(int health) => _healthText.text = $"{health}/{_healthManager.MaxHealth}";
    public void UpdateMoney(int amount) => _moneyText.text = amount.ToString();
}
