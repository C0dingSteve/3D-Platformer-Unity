using System;
using Assets.Scripts.ServiceLocator;
using UnityEngine;

public class AudioManager : BaseSingleton<AudioManager>, IAudioManagerService
{
    private AudioData[] _musicDataArray;
    private AudioData[] _sfxDataArray;

    private AudioSource _audioSource;

    protected override void Initialize()
    {
        AudioData[] audioData = Resources.LoadAll<AudioData>("AudioData");

        _sfxDataArray = Array.FindAll(audioData, x => x.clipType == AudioClipType.SFX);
        _musicDataArray = Array.FindAll(audioData, x => x.clipType == AudioClipType.BACKGROUND);
        
        _audioSource = gameObject.AddComponent<AudioSource>();
        
        ServiceLocator.Register(this);
    }

    public void FadeIn(AudioData data, float duration) => throw new NotImplementedException();
    public void FadeOut(int playbackId, float duration) => throw new NotImplementedException();
    public bool IsPlaying(int playbackId) => throw new NotImplementedException();
    public void Mute(AudioClipType type, bool isMuted) => throw new NotImplementedException();
    public void Pause(int playbackId) => throw new NotImplementedException();
    public int Play(AudioData data, bool loop = false) => throw new NotImplementedException();
    public int PlayAtPoint(AudioData data, Vector3 position, float spatialBlend = 1) => throw new NotImplementedException();
    public void Resume(int playbackId) => throw new NotImplementedException();
    public void SetVolume(AudioClipType type, float volume) => throw new NotImplementedException();
    public void Stop(int playbackId) => throw new NotImplementedException();
    public void StopAll(AudioClipType type) => throw new NotImplementedException();
}