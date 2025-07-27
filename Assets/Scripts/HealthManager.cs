using System;
using UnityEngine;

public class HealthManager: MonoBehaviour
{
    public static HealthManager Instance { get; private set; }

    [SerializeField] private int _currentHealth, _maxHealth = 5;
    public int CurrentHealth => _currentHealth;
    public int MaxHealth => _maxHealth;

    // Events to notify other components/systems when health changes
    public event Action<int> OnDamaged;
    public event Action<int> OnHealed;
    public event Action OnDeath;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _currentHealth = _maxHealth;
    }

    public void TakeDamage(int damageAmount)
    {
        _currentHealth = Mathf.Clamp(_currentHealth - damageAmount, 0, _maxHealth);

        if (_currentHealth <= 0)
        {
            Kill();
        }
        else
        {
            // Notify subscribers that the player took some damage
            OnDamaged?.Invoke(_currentHealth);
        }
    }

    public void Kill()
    {
        _currentHealth = 0;
        OnDeath?.Invoke();
        GameManager.Instance.Respawn();
    }

    public void Heal(int healAmount)
    {
        if (healAmount == MaxHealth) return;

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
