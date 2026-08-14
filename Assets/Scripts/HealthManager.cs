using System;
using Assets.Scripts.ServiceLocator;
using UnityEngine;

public class HealthManager: MonoBehaviour
{
    [SerializeField] private int _currentHealth, _maxHealth = 5;
    public int CurrentHealth => _currentHealth;
    public int MaxHealth => _maxHealth;

    // Events to notify other components/systems when health changes
    public event Action<int> OnDamaged;
    public event Action<int> OnHealed;
    public event Action OnDeath;

    private void Awake()
    {
        ServiceLocator.Register(this);
        _currentHealth = _maxHealth;
    }
    
    public void TakeDamage(int damageAmount)
    {
        _currentHealth = Mathf.Clamp(_currentHealth - damageAmount, 0, _maxHealth);

        if (_currentHealth <= 0) Kill();
        else OnDamaged?.Invoke(_currentHealth); // Notify subscribers player took damage
    }

    public void Kill()
    {
        _currentHealth = 0;
        OnDeath?.Invoke();
        ServiceLocator.Get<GameManager>().Respawn();
    }

    public void Heal(int healAmount)
    {
        if (_currentHealth == _maxHealth) return;

        _currentHealth = Mathf.Clamp(_currentHealth + healAmount, 0, _maxHealth);
        OnHealed?.Invoke(_currentHealth);
    }

    public void ResetHealth()
    {
        if (_currentHealth == _maxHealth) return;
        _currentHealth = _maxHealth;
        OnHealed?.Invoke(_currentHealth);
    }
}
