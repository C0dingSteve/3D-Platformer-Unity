using Assets.Scripts.ServiceLocator;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneLoader: BaseSingleton<SceneLoader>
{
    private LevelMapping _levelMapping;
    
    protected override void Initialize()
    {
        ServiceLocator.Register(this);
        _levelMapping = Resources.Load<LevelMapping>("LevelMapping");
        if(_levelMapping == null)
        {
            Debug.LogError($"{gameObject.name} LevelMapping asset not found in Resources Folder");
        }
    }

    public void LoadNextSceneInBuild()
    {
        int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if(nextIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextIndex);
        }
        else
        {
            Debug.LogError($"{gameObject.name} Already at the last scene in build settings");
        }
    }
    public void LoadLevel(GameLevel level)
    {
        if(_levelMapping == null) return;

        
        if(_levelMapping.LevelBuildIndexMapping.TryGetValue(level, out int buildIndex))
        {
            if(buildIndex >= 0 && buildIndex < SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadScene(buildIndex);
            }
        }
        else
        {
            Debug.LogError($"{gameObject.name} No mapping found for level: {level}");
        }
    }

}