using UnityEngine;

public interface IAudioManagerService
{
    // Category Control
    void SetVolume(AudioClipType type, float volume);
    void Mute(AudioClipType type, bool isMuted);
    void StopAll(AudioClipType type);

    // Playback (Returns Instance ID for control)
    int Play(AudioData data, bool loop = false);
    int PlayAtPoint(AudioData data, Vector3 position, float spatialBlend = 1.0f);

    // Instance Control
    void Stop(int playbackId);
    void Pause(int playbackId);
    void Resume(int playbackId);
    bool IsPlaying(int playbackId);

    // Transitions
    void FadeOut(int playbackId, float duration);
    void FadeIn(AudioData data, float duration);
}
