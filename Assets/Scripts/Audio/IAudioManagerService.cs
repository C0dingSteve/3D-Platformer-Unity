using UnityEngine;

public interface IAudioManagerService
{
    // Category Control
    void SetVolume(Enum type, float volume);
    void Mute(Enum type, bool isMuted);
    void StopAll(Enum type);

    // Playback (Returns Instance ID for control)
    int Play(IAudioData audioData, bool loop = false);
    int PlayAtPoint(IAudioData audioData, Vector3 position, float spatialBlend = 1.0f);

    // Instance Control
    void Stop(int playbackId);
    void Pause(int playbackId);
    void Resume(int playbackId);
    bool IsPlaying(int playbackId);

    // Transitions
    void FadeOut(int playbackId, float duration);
    void FadeIn(IAudioData data, float duration);
}
