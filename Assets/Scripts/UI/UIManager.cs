using Assets.Scripts.ServiceLocator;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameHUD _gameHUD;
    private ScreenFade _screenFade;

    private void Awake()
    {
        _screenFade = gameObject.AddComponent<ScreenFade>();
    }

    public void InitializeHUD()
    {
        _gameHUD.Initialize();
        _gameHUD.UpdateHealth(ServiceLocator.Get<HealthManager>().MaxHealth);
        _gameHUD.UpdateMoney(ServiceLocator.Get<GameManager>().Money);
    }

    public void UpdateMoney(int amount) => _gameHUD.UpdateMoney(amount);
    public void UpdateHealth(int health) => _gameHUD.UpdateHealth(health);

    public void FadeScreen()
    {
        _screenFade.Play();
    }
}
