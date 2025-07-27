using UnityEngine;

public class HealEffect : HealthEventSubscriber
{
    private TimedAction _healAction;

    void Awake()
    {
        _healAction = gameObject.AddComponent<TimedAction>();
        NullChecker.Check(_healAction);
    }

    protected override void HandleHealing(int health)
    {
        // player effect for healing
    }
}
