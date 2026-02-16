using System.IO;
using UnityEditor;
using UnityEngine;

public static class AudioDataUtility
{
    private const string MUSIC_FOLDER = "Assets/_Raw/Audio/Music";
    private const string SFX_FOLDER = "Assets/_Raw/Audio/SFX";
    private const string DATA_OUTPUT_FOLDER = "Assets/Resources/AudioData";

    [MenuItem("Tools/Audio/Generate AudioData Assets")]
    public static void GenerateAudioData()
    {
        EnsureFolderExists(DATA_OUTPUT_FOLDER);

        string[] guids = AssetDatabase.FindAssets("t:AudioClip", new[] { MUSIC_FOLDER, SFX_FOLDER });

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            AudioClip clip = AssetDatabase.LoadAssetAtPath<AudioClip>(path);
            string assetPath = Path.Combine(DATA_OUTPUT_FOLDER, $"{clip.name}.asset");

            if (AssetDatabase.LoadAssetAtPath<AudioData>(assetPath) != null) continue;

            AudioData data = ScriptableObject.CreateInstance<AudioData>();
            data.clip = clip;
            data.name = clip.name;

            if (path.StartsWith(MUSIC_FOLDER)) data.clipType = AudioClipType.BACKGROUND;
            else if (path.StartsWith(SFX_FOLDER)) data.clipType = AudioClipType.SFX;

            AssetDatabase.CreateAsset(data, assetPath);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Batch AudioData generation complete.");
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