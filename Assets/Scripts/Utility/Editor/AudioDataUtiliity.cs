using System.IO;
using UnityEditor;
using UnityEngine;

public static class AudioDataUtility
{
    private const string MUSIC_FOLDER = "Assets/_Raw/Audio/Music";
    private const string SFX_FOLDER = "Assets/_Raw/Audio/SFX";
    
    private const string MUSIC_OUTPUT_FOLDER = "Assets/Resources/AudioData/Music";
    private const string SFX_OUTPUT_FOLDER = "Assets/Resources/AudioData/Sfx";

    [MenuItem("Tools/Audio/Generate AudioData Assets")]
    public static void GenerateAudioData()
    {
        EnsureFolderExists(MUSIC_OUTPUT_FOLDER);
        EnsureFolderExists(SFX_OUTPUT_FOLDER);

        GenerateAssetsForFolder(MUSIC_FOLDER, MUSIC_OUTPUT_FOLDER);
        GenerateAssetsForFolder(SFX_FOLDER, SFX_OUTPUT_FOLDER);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Batch AudioData generation complete.");
    }

    private static void GenerateAssetsForFolder(string folderPath, string outputFolder)
    {
        var guids = AssetDatabase.FindAssets("t:AudioClip", new[] { folderPath });
        foreach (var guid in guids)
        {
            CreateAndSaveAsset(guid, outputFolder);
        }
    }

    private static void CreateAndSaveAsset(string guid, string outputFolder)
    {
        var clip = AssetDatabase.LoadAssetAtPath<AudioClip>(AssetDatabase.GUIDToAssetPath(guid));
        if (clip == null) return;

        string assetPath = Path.Combine(outputFolder, $"{clip.name}.asset");
        ScriptableObject data = outputFolder switch
        {
            MUSIC_OUTPUT_FOLDER => CreateMusicDataInstance(clip),
            SFX_OUTPUT_FOLDER => CreateSfxDataInstance(clip),
            _ => null            
        };

        if (data != null)
        {
            AssetDatabase.CreateAsset(data, assetPath);
        }
        else
        {
            Debug.LogError("Failed to create Music/Sfx Data");
        }
    }

    private static MusicData CreateMusicDataInstance(AudioClip clip, AudioCategory type = AudioCategory.None, float volume = 1f)
    {
        return CreateAudioDataInstance<MusicData>(clip, type, volume);
    }

    private static SfxData CreateSfxDataInstance(AudioClip clip, SfxType type = SfxType.None, float volume = 1f)
    {
        return CreateAudioDataInstance<SfxData>(clip, type, volume, true);
    }

    private static T CreateAudioDataInstance<T>(AudioClip clip, System.Enum type, float volume, bool isSfx = false) where T : ScriptableObject
    {
        var data = ScriptableObject.CreateInstance<T>();
        if (data is MusicData musicData)
        {
            musicData.clip = clip;
            musicData.musicType = (AudioCategory)type;
            musicData.volume = volume;
        }
        else if (data is SfxData sfxData)
        {
            sfxData.clips = new AudioClip[] { clip };
            sfxData.sfxType = (SfxType)type;
            sfxData.volume = volume;
        }
        data.name = clip.name;

        return data;
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
