using UnityEditor;
using UnityEngine;

[System.Serializable]
public struct LevelEntry
{
    public GameLevel level;
#if UNITY_EDITOR
    public SceneAsset sceneAsset;
#endif
    [HideInInspector] public string sceneName;
    [HideInInspector] public int buildIndex;
}
