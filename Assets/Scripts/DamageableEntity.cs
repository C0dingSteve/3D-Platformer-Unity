using Assets.Scripts.Utility;
using UnityEngine;

[RequireComponent(typeof(HealthManager))]
public class DamageableEntity : MonoBehaviour
{
    [SerializeField] private InvincibilityEffect _invincibilityEffect;

    private void Awake()
    {
        if (!_invincibilityEffect.IsNull())
            _invincibilityEffect = GetComponent<InvincibilityEffect>();
    }

    public void ApplyDamage(int amount)
    {
        if (!_invincibilityEffect.IsNull() || _invincibilityEffect.IsCurrentlyInvincible)
            return;

        GetComponent<HealthManager>().TakeDamage(amount);
    }
}
