using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(UIManager))]
public class ScreenFade: Transition
{
    private Image _blackScreen;
    private Color _screenColor;

    protected override void Awake()
    {
        base.Awake();
        CreateAndSetupOverlay();
    }

    private void CreateAndSetupOverlay()
    {
        // Create Overlay
        GameObject overlay = new GameObject("FadeOverlay");
        overlay.transform.SetParent(transform, false);

        _blackScreen = overlay.AddComponent<Image>();

        // Stretch to fill
        RectTransform rect = _blackScreen.rectTransform;
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.sizeDelta = Vector2.zero;

        // Setup Overlay
        _screenColor = Color.black;
        _screenColor.a = 0f;
        _blackScreen.color = _screenColor;
        _blackScreen.raycastTarget = false;
    }

    public override sealed void Play()
    {
        float _fadeProgression = 0f;
        float _halfDuration = _duration / 2f;

        _timedAction.RunAction(_duration, () =>
            {
                float targetAlpha = (_fadeProgression < _halfDuration) ? 1f:0f;
                _screenColor.a = Mathf.MoveTowards(_screenColor.a, targetAlpha, _halfDuration * Time.deltaTime);
                _blackScreen.color = _screenColor;
                _fadeProgression += Time.deltaTime * _halfDuration;
            }
        );
    }
}