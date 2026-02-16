using System;
using Assets.Scripts.ServiceLocator;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button playButton;
    public Button settingsButton;
    public Button quitButton;

    private SceneLoader _sceneLoader;

    private void Awake()
    {
        playButton.onClick.AddListener(PlayGame);
        settingsButton.onClick.AddListener(OpenSettingsMenu);
        quitButton.onClick.AddListener(QuitGame);

        _sceneLoader = ServiceLocator.Get<SceneLoader>();
    }

    private void PlayGame() => _sceneLoader.LoadGame();

    private void OpenSettingsMenu() => throw new NotImplementedException();
    private void QuitGame() => throw new NotImplementedException();
}
