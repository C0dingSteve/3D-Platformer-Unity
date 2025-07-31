using System.IO;
using UnityEditor;
using UnityEngine;

public class AudioDataFactory : MonoBehaviour
{
    [field: SerializeField] public string MusicFolder { get; private set; }
    [field: SerializeField] public string SfxFolder { get; private set; }
    
    [SerializeField] private string _audioDataFolder = "Assets/AudioData";
    public string AudioDataFolder => _audioDataFolder;

    [ContextMenu("Create AudioData Assets")]
    public void CreateAudioDataAssets()
    {
        if (!Directory.Exists(MusicFolder))
            throw new DirectoryNotFoundException($"Music folder not found: {MusicFolder}");

        if (!Directory.Exists(SfxFolder))
            throw new DirectoryNotFoundException($"sfx folder nor found: {SfxFolder}");

        string[] guids = AssetDatabase.FindAssets("t:AudioClip", new[] { MusicFolder, SfxFolder });

        if (!Directory.Exists(_audioDataFolder))
        {
            Directory.CreateDirectory(_audioDataFolder);
            AssetDatabase.Refresh();
        }

        foreach (string guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            AudioClip audioClip = AssetDatabase.LoadAssetAtPath<AudioClip>(assetPath);

            if (AssetDatabase.LoadAssetAtPath<AudioData>($"{_audioDataFolder}/{audioClip.name}.asset") != null) continue;

            AudioData audioData = ScriptableObject.CreateInstance<AudioData>();
            audioData.audioClip = audioClip;
            audioData.name = audioClip.name;

            if (assetPath.StartsWith(MusicFolder))
                audioData.audioType = AudioClipType.BACKGROUND;
            else if (assetPath.StartsWith(SfxFolder))
                audioData.audioType = AudioClipType.SFX;

            // Create the asset in the specified folder
            string audioDataPath = $"Assets/AudioData/{audioData.name}.asset";
            AssetDatabase.CreateAsset(audioData, audioDataPath);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("AudioData assets created successfully!");
    }
}
