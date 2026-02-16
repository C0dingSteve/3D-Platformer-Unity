using UnityEngine;

public enum AudioClipType
{
    BACKGROUND, SFX, DIALOGUE
}

[CreateAssetMenu(fileName = "AudioData", menuName = "ScriptableObject/AudioData")]
public class AudioData : ScriptableObject
{
    public AudioClip clip;
    public AudioClipType clipType;
    public float volume = 1.0f;
}
