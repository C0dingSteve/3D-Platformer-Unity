using Assets.Scripts.ServiceLocator;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private ScreenFade _screenFade;

    private void Awake()
    {
        ServiceLocator.Register(this);
        _screenFade = gameObject.AddComponent<ScreenFade>();
    }

    public void FadeScreen() => _screenFade.Play();
}
