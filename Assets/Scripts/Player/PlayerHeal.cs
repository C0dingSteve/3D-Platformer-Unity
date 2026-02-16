using Assets.Scripts.Utility;
using UnityEngine;

[RequireComponent(typeof(HealthManager))]
public class PlayerHeal : HealthEventSubscriber
{
    private TimedAction _healAction;

    protected override void Awake()
    {
        base.Awake();
        _healAction = gameObject.AddComponent<TimedAction>();
        _healAction.LogNullStatus();
    }

    protected override void HandleHealing(int health)
    {
        // player effect for healing
    }
}
