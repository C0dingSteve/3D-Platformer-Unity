using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "LevelMapping", menuName = "ScriptableObject/Level Mapping")]
public class LevelMapping: ScriptableObject, ISerializationCallbackReceiver
{
    [SerializeField]
    private List<LevelEntry> _levelEntries = new();

    public Dictionary<GameLevel, int> LevelBuildIndexMapping {get; private set;} = new();

    public void OnAfterDeserialize() => SyncDictionary();
    
    private void SyncDictionary()
    {
        LevelBuildIndexMapping.Clear();
        foreach(LevelEntry entry in _levelEntries)
            LevelBuildIndexMapping[entry.level] = entry.buildIndex;
    }

    public void OnBeforeSerialize() { }

#if UNITY_EDITOR
    public void OnValidate()
    {
        for(int i = 0; i < _levelEntries.Count; i++)
        {
            LevelEntry entry = _levelEntries[i];
            if(entry.sceneAsset != null)
            {
                entry.sceneName = entry.sceneAsset.name;
                string path = AssetDatabase.GetAssetPath(entry.sceneAsset);
                entry.buildIndex = SceneUtility.GetBuildIndexByScenePath(path);
            }
            _levelEntries[i] = entry;
        }
        SyncDictionary();
    }
#endif
}