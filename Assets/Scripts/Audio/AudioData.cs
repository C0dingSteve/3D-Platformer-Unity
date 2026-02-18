using UnityEngine;

[CreateAssetMenu(fileName = "AudioData", menuName = "ScriptableObject/AudioData")]
public class AudioData : ScriptableObject
{
    public AudioClip clip;
    public AudioClipType clipType;
    public float volume = 1.0f;

    [HideInInspector]
    public int instanceID;

#if UNITY_EDITOR
    private void OnValidate()
    {
        instanceID = clip.GetInstanceID();
    }
#endif
}
