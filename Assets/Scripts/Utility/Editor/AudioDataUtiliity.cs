using System.IO;
using UnityEditor;
using UnityEngine;

public static class AudioDataUtility
{
    private const string MUSIC_FOLDER = "Assets/_Raw/Audio/Music";
    private const string SFX_FOLDER = "Assets/_Raw/Audio/SFX";
    
    private const string MUSIC_OUTPUT_FOLDER = "Assets/Resources/AudioData/Music";
    private const string SFX_OUTPUT_FOLDER = "Assets/Resources/AudioData/Sfx";
    // private const string DIALOGUE_OUTPUT_FOLDER = "Assets/Resources/AudioData/Sfx";

    [MenuItem("Tools/Audio/Generate AudioData Assets")]
    public static void GenerateAudioData()
    {
        EnsureFolderExists(MUSIC_OUTPUT_FOLDER);
        EnsureFolderExists(SFX_OUTPUT_FOLDER);
        // EnsureFolderExists(DIALOGUE_OUTPUT_FOLDER);

        string[] guids = AssetDatabase.FindAssets("t:AudioClip", new[] { MUSIC_FOLDER, SFX_FOLDER });

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            AudioClip clip = AssetDatabase.LoadAssetAtPath<AudioClip>(path);
            AudioClipType clipType = GetAudioClipType(path);
            
            string assetPath = clipType switch
            {
                AudioClipType.MUSIC => Path.Combine(MUSIC_OUTPUT_FOLDER, $"{clip.name}.asset"),
                AudioClipType.SFX => Path.Combine(SFX_OUTPUT_FOLDER, $"{clip.name}.asset"),
                // AudioClipType.DIALOGUE => Path.Combine(DIALOGUE_OUTPUT_FOLDER, $"{clip.name}.asset"),
                _ => string.Empty
            };

            // if (AssetDatabase.LoadAssetAtPath<AudioData>(assetPath) != null) continue;
            if (assetPath == string.Empty) continue;

            AudioData data = CreateAudioDataInstance(clip, clipType);
            
            AssetDatabase.CreateAsset(data, assetPath);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Batch AudioData generation complete.");
    }

    private static AudioData CreateAudioDataInstance(AudioClip clip, AudioClipType clipType, float volume = 1f)
    {
        AudioData data = ScriptableObject.CreateInstance<AudioData>();
        data.clip = clip;
        data.clipType = clipType;
        data.name = clip.name;
        data.volume = volume;

        return data;
    }

    private static AudioClipType GetAudioClipType(string path)
    {
        if(path.StartsWith(MUSIC_FOLDER)) return AudioClipType.MUSIC;
        if(path.StartsWith(SFX_FOLDER)) return AudioClipType.SFX;

        throw new InvalidDataException($"Invalid Path: {path}");
    }

    private static void EnsureFolderExists(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
            AssetDatabase.Refresh();
        }
    }
}