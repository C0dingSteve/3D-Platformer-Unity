using UnityEngine;

[CreateAssetMenu(fileName = "SFXData", menuName = "ScriptableObject/Audio/SFX Data")]
public class SfxData: ScriptableObject, IAudioData
{
    public SfxType sfxType;
    public AudioClip[] clips;

    [Range(0, 1)] public float volume = 1f;
    [Range(0.1f, 2f)] public float pitchMin = 0.95f;
    [Range(0.1f, 2f)] public float pitchMax = 1.05f;
}
