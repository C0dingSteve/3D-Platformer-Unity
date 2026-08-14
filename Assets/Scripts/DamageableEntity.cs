using Assets.Scripts.Utility;
using UnityEngine;

[RequireComponent(typeof(HealthManager))]
public class DamageableEntity : MonoBehaviour
{
    [SerializeField] private PlayerInvincibleEffect _playerInvincibleEffect;

    private void Awake()
    {
        if (_playerInvincibleEffect.IsNull() == null)
            _playerInvincibleEffect = GetComponent<PlayerInvincibleEffect>();
    }

    public void ApplyDamage(int amount)
    {
        if (_playerInvincibleEffect.IsNull() != null && _playerInvincibleEffect.IsCurrentlyInvincible)
            return;

        GetComponent<HealthManager>().TakeDamage(amount);
    }
}
