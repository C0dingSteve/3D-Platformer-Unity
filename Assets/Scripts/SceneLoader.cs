using Assets.Scripts.ServiceLocator;
using UnityEngine.SceneManagement;

public class SceneLoader: BaseSingleton<SceneLoader>
{
    protected override void Initialize() => ServiceLocator.Register(this);
    public void LoadGame() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
}