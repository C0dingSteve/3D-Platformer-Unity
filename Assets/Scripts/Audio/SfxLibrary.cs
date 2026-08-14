using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SfxLibrary", menuName = "ScriptableObject/Audio/SfxLibrary")]
public class SfxLibrary: ScriptableObject, ISerializationCallbackReceiver
{
    [SerializeField] private List<SfxData> sfxList = new();
    public Dictionary<SfxType, SfxData> Mapping {get; private set;} = new();

    public void OnAfterDeserialize()
    {
        Mapping.Clear();
        foreach(SfxData sfx in sfxList)
        {
            if(sfx != null) Mapping.TryAdd(sfx.sfxType, sfx);
        }
    }

    public void OnBeforeSerialize() { }
}