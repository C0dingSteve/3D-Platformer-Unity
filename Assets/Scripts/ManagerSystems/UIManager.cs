using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Utility;

public class UIManager : HealthEventSubscriber
{
    [SerializeField] private Image _blackScreen;
    [SerializeField] private float _fadeDuration = 2f;
    [SerializeField] private TMP_Text _healthText;
    [SerializeField] private TMP_Text _moneyText;

    private TimedAction _screenFadeAction;

    private Color _screenColor;

    // Changing this to Start probably causes initialization
    // order problem with HealthEventSubscriber - NEED FIXING 
    void Awake()  
    {
        _screenFadeAction = gameObject.AddComponent<TimedAction>();
        _screenFadeAction.LogNullStatus();

        _screenColor = _blackScreen.color;
    }

    public void ActivateScreenFade()
    {
        float _fadeProgression = 0f;
        float _halfDuration = _fadeDuration / 2f;

        _screenFadeAction.RunAction(_fadeDuration, () =>
            {
                float targetAlpha = (_fadeProgression < _halfDuration) ? 1f:0f;
                _screenColor.a = Mathf.MoveTowards(_screenColor.a, targetAlpha, _halfDuration * Time.deltaTime);
                _blackScreen.color = _screenColor;
                _fadeProgression += Time.deltaTime * _halfDuration;
            }
        );
    }

    protected override void HandleDamage(int health) => UpdateUIHealth(health);
    protected override void HandleHealing(int health) => UpdateUIHealth(health);
    protected override void HandleDeath() => UpdateUIHealth(0);

    public void UpdateUIHealth(int health)
    {
        Debug.Log(health/HealthManager.Instance.MaxHealth);
        _healthText.text = $"{health}/{HealthManager.Instance.MaxHealth}";
    }

    public void UpdateUIMoney(int money)
    {
        _moneyText.text = money.ToString();
    }
}
