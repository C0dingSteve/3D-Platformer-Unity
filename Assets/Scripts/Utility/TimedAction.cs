using System;
using System.Collections;
using UnityEngine;

public class TimedAction : MonoBehaviour
{
    public bool IsActive { get; private set; } = false;

    private Coroutine _currentActionCo;

    public void RunAction(float duration, Action actionPerFrame = null, Action actionOnCompletion = null)
    {
        if (IsActive) return;

        duration = Mathf.Max(duration, 0);
        _currentActionCo = StartCoroutine(ActionCo(duration, actionPerFrame, actionOnCompletion));
    }

    public void StopAction()
    {
        if (_currentActionCo != null)
        {
            StopCoroutine(_currentActionCo);
            _currentActionCo = null;
            IsActive = false;
        }
    }

    private IEnumerator ActionCo(float duration, Action actionPerFrame = null, Action actionOnCompletion = null)
    {
        IsActive = true;
        float _timeRemaining = duration;

        try
        {
            while (_timeRemaining > 0)
            {
                _timeRemaining -= Time.deltaTime;
                actionPerFrame?.Invoke();
                yield return null;
            }

            _timeRemaining = 0;

            actionOnCompletion?.Invoke();
        }
        finally
        {
            IsActive = false;
            _currentActionCo = null;
        }
    }
}
