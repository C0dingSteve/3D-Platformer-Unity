using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : HealthEventSubscriber
{
    [SerializeField] private Image _blackScreen;
    [SerializeField] private float _fadeDuration = 2f;
    [SerializeField] private TMP_Text healthText;

    private TimedAction _screenFadeAction;

    private Color _screenColor;

    void Start()
    {
        _screenFadeAction = gameObject.AddComponent<TimedAction>();
        NullChecker.Check(_screenFadeAction);

        _screenColor = _blackScreen.color;
    }

    public void ActivateScreenFade()
    {
        float _fadeProgression = 0f;
        float _halfDuration = _fadeDuration / 2f;

        _screenFadeAction.RunAction(_fadeDuration, () =>
            {
                if (_fadeProgression < _halfDuration)
                    _screenColor.a = Mathf.MoveTowards(_screenColor.a, 1f, _halfDuration * Time.deltaTime);
                else
                    _screenColor.a = Mathf.MoveTowards(_screenColor.a, 0f, _halfDuration * Time.deltaTime);

                _blackScreen.color = _screenColor;

                _fadeProgression += Time.deltaTime * _halfDuration;
            }
        );
    }

    protected override void HandleDamage(int health)
    {
        UpdateUIHealth(health);
    }

    protected override void HandleHealing(int health)
    {
        UpdateUIHealth(health);
    }

    protected override void HandleDeath()
    {
        UpdateUIHealth(0);
    }

    public void UpdateUIHealth(int health)
    {
        healthText.text = health.ToString();
    }
}
