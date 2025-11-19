using UnityEngine;

public enum AudioClipType
{
    BACKGROUND, SFX, DIALOGUE
}

[CreateAssetMenu(fileName = "AudioData", menuName = "ScriptableObject/AudioData")]
public class AudioData : ScriptableObject
{
    public AudioClip audioClip;
    public AudioClipType audioType;
    public float volume = 1.0f;

    public void PlayAudio(Vector3 position)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volume);
    }
}
