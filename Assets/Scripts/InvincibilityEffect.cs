using System;
using UnityEngine;
using Assets.Scripts.Utility;

public class InvincibilityEffect : HealthEventSubscriber
{
    [Header("Invincibility")]
    [SerializeField] GameObject[] _playerPieces;
    [SerializeField] private float _invincibilityDuration = 2f;
    [SerializeField] private float _flashingRate = 0.05f;
    public bool IsCurrentlyInvincible { get; private set; } = false;

    private TimedAction _invincibilityTimedAction;

    [Header("Knockback")]
    private PlayerController _playerController; // Reference to PlayerController on *this* GameObject

    private void Awake()
    {
        _invincibilityTimedAction = gameObject.AddComponent<TimedAction>();
        _invincibilityTimedAction.LogNullStatus();

        _playerController = GetComponent<PlayerController>();
        _playerController.LogNullStatus();
    }

    public void ActivateEffect()
    {
        if (IsCurrentlyInvincible)
            return;

        IsCurrentlyInvincible = true;
        float rate = _flashingRate;

        _invincibilityTimedAction.RunAction(
            _invincibilityDuration,
            () =>
            {
                if (rate <= 0f)
                {
                    rate = _flashingRate;
                    // Array.ForEach(_playerPieces, piece => { piece.SetActive(!piece.activeSelf); });
                    foreach (GameObject p in _playerPieces) p.SetActive(!p.activeSelf);
                }
                rate -= Time.deltaTime;
            },
            () =>
            {
                Array.ForEach(_playerPieces, piece => { piece.SetActive(true); });
                IsCurrentlyInvincible = false;
            }
        );
    }

    public void StopEffect()
    {
        if (_invincibilityTimedAction != null && _invincibilityTimedAction.IsActive)
        {
            _invincibilityTimedAction.StopAction();
            IsCurrentlyInvincible = false;
        }
    }

    protected override void HandleDamage(int health)
    {
        ActivateEffect();
        _playerController.ActivateKnockBack();
    }
}
