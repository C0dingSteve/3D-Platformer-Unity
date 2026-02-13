using UnityEngine;

[RequireComponent(typeof(GameHUD))]
public class HealthHUDBridge: HealthEventSubscriber
{
    private IGameHUD _gameHUD;

    private void Awake() => _gameHUD = GetComponent<GameHUD>();

    protected override void HandleDamage(int health) => _gameHUD.UpdateHealth(health);
    protected override void HandleHealing(int health) => _gameHUD.UpdateHealth(health);
    protected override void HandleDeath() => _gameHUD.UpdateHealth(0);
}