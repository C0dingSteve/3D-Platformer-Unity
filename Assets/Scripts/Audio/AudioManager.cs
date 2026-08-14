using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.Scripts.ServiceLocator;
using System.Collections.Generic;
using System.Linq;

public class AudioManager : BaseSingleton<AudioManager>, IAudioManagerService
{
    private SfxData[] _sfxDataArray;

    private AudioSource _audioSource;

    private Dictionary<string, SceneAudioConfig> _sceneMusicDict;
    private string _lastPlayedScene;

    protected override void Initialize()
    {
        ServiceLocator.Register(this);

        _audioSource = gameObject.AddComponent<AudioSource>();

        _sfxDataArray = Resources.LoadAll<SfxData>("AudioData/Sfx");
        _sceneMusicDict = Resources
                            .LoadAll<SceneAudioConfig>("AudioData/SceneAudioConfigs")
                            .ToDictionary(item => item.sceneName, item => item);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

    /// <summary>
    /// Determines if the loaded scene is a "Primary" level load.
    /// 
    /// TECHNICAL CONTEXT:
    /// - 0 (Single): Standard runtime behavior for LoadSceneMode.Single.
    /// - 1 (Additive): Standard runtime behavior for LoadSceneMode.Additive.
    /// - 4 (Undocumented): An Editor-only flag leaked from Unity's native C++ core.
    ///   It occurs specifically when hitting 'Play' in the Editor (Initial Play Mode).
    /// 
    /// WHY THIS CHECK EXISTS:
    /// In the Editor, the first scene triggers a value of 4. Subsequent loads trigger 0.
    /// In Production builds, only 0 and 1 are returned. This helper unifies both
    /// environments to ensure music initializes on startup and scene transitions.
    /// </summary>
    private bool IsPrimaryLoad(LoadSceneMode mode)
    {
        // 0 is the standard 'Single' mode in all environments
        if (mode == LoadSceneMode.Single) return true;

    #if UNITY_EDITOR
        // 4 is the 'Initial Play Mode' value leaked by the Editor
        if ((int)mode == 4) return true;
    #endif

        return false;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        if(!IsPrimaryLoad(sceneMode)) return;

        // Safe lookup: Avoid crash if scene has no config
        if(!_sceneMusicDict.TryGetValue(scene.name, out SceneAudioConfig config))
        {
            Debug.LogError($"Scene {scene.name} has no SceneAudioConfig");
            return;
        }

        if(config.musicTracks == null || config.musicTracks.Length == 0)
        {
            Debug.LogError($"Scene Audio Data Not Valid");
            return;
        }

        if(_audioSource.clip != null)
        {
            // Implement and use IsPlaying() here later
            bool isAlreadyPlaying = config.musicTracks.Any(data => data.clip == _audioSource.clip);
            if(isAlreadyPlaying) return;
        }

        _lastPlayedScene = scene.name;
        
        Play(config.musicTracks[0], config.loop);
        SetVolume(AudioCategory.None, 0.025f);
    }

    private void ApplyMusicData(MusicData data, bool changeVolume = false)
    {
        _audioSource.clip = data.clip;
        
        // Reset spatial blend to 2D by default unless PlayAtPoint overrides it
        _audioSource.spatialBlend = 0;
        
        if(changeVolume) SetVolume(data.musicType, data.volume);
    }

    private int PlayMusic(MusicData data, bool loop)
    {
        ApplyMusicData(data, changeVolume: true);
        
        _audioSource.loop = loop;
        _audioSource.Play();

        return _audioSource.clip.GetInstanceID();
    }

    private int PlaySfx(SfxData data)
    {
        throw new NotImplementedException();
    }

    // +++++++++++++++ Interface Implementation +++++++++++++++ 

    public void FadeIn(IAudioData data, float duration) => throw new NotImplementedException();

    public void FadeOut(int playbackId, float duration) => throw new NotImplementedException();

    public bool IsPlaying(int playbackId) => throw new NotImplementedException();

    public void Mute(AudioCategory type, bool isMuted) => throw new NotImplementedException();

    public void Pause(int playbackId)
    {
        if(_audioSource.clip.GetInstanceID() != playbackId) 
            throw new InvalidDataException($"Given ID: ({playbackId}) != AudioSource clip ID: {_audioSource.clip.GetInstanceID()}");
        if(_audioSource.isPlaying)
            _audioSource.Pause();
    }

    public int Play(IAudioData data, bool loop = false)
    {
        return PlayMusic((MusicData)data, loop);
    }

    public int PlayAtPoint(IAudioData data, Vector3 position, float spatialBlend = 1)
    {
        _audioSource.transform.position = position;
        _audioSource.spatialBlend = spatialBlend;

        return Play(data, _audioSource.loop);
    }

    public void Resume(int playbackId)
    {
        if(_audioSource.clip.GetInstanceID() != playbackId) 
            throw new InvalidDataException($"Given ID: ({playbackId}) != AudioSource clip ID: {_audioSource.clip.GetInstanceID()}");
        if(!_audioSource.isPlaying)
            _audioSource.UnPause();
    }

    public void SetVolume(AudioCategory type, float volume)
    {
        // _audioSource.volume = type switch
        // {
        //     AudioClipType.MUSIC => volume * 0.6f,
        //     AudioClipType.SFX => volume,
        //     _ => throw new InvalidDataException($"Invalid Audio Type: {type}")
        // };
        _audioSource.volume = volume * 0.6f;
    }

    public void Stop(int playbackId) => throw new NotImplementedException();

    public void StopAll(AudioCategory type) => throw new NotImplementedException();
}
