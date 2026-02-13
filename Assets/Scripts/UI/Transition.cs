using UnityEngine;
using Assets.Scripts.Utility;

[RequireComponent(typeof(TimedAction))]
public abstract class Transition: MonoBehaviour
{
    [SerializeField] protected float _duration = 2f;
    protected TimedAction _timedAction;

    protected virtual void Awake()
    {
        _timedAction = GetComponent<TimedAction>();
        if(_timedAction.IsNull() == null) 
            _timedAction = gameObject.AddComponent<TimedAction>();
    }

    public abstract void Play();
}