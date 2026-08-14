using UnityEngine;

[CreateAssetMenu(fileName = "MusicData", menuName = "ScriptableObject/Audio/Music Data")]
public class MusicData: ScriptableObject, IAudioData
{
    public AudioCategory musicType;
    public AudioClip clip;
    public bool loop = true;
    [Range(0, 1)] public float volume = 1f;
}