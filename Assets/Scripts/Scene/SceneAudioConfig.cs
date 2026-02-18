using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "NewSceneAudioConfig", menuName = "ScriptableObject/Scene Audio Config")]
public class SceneAudioConfig: ScriptableObject
{
#if UNITY_EDITOR 
    // This field is for the Inspector only and is stripped in builds
    public SceneAsset sceneAsset;
#endif

    // This string persists in production builds
    [HideInInspector]
    public string sceneName;

    public AudioData[] sceneAudioDatas;
    public bool loop = true;

    private void OnValidate()
    {
        #if UNITY_EDITOR
        if(sceneAsset != null)
        {
            sceneName = sceneAsset.name;
        }
        #endif
    }
}