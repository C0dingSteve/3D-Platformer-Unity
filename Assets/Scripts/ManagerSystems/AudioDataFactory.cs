using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class AudioDataFactory : MonoBehaviour
{
    [field: SerializeField] public string MusicFolder { get; private set; }
    [field: SerializeField] public string SfxFolder { get; private set; }
    
    [SerializeField] private string _audioDataFolder = "Assets/AudioData";
    public string AudioDataFolder => _audioDataFolder;

    private string[] audioClipGUIDs;

    private bool LoadAudioClips()
    {
        audioClipGUIDs = new string[0];

        Dictionary<string, string> folders = new()
        {
            { "Music Folder", MusicFolder },
            { "SFX Folder", SfxFolder }
        };

        foreach (KeyValuePair<string, string> folder in folders)
        {
            if (Directory.Exists(folder.Value)) continue;

            Debug.LogWarning($"{folder.Key} not found: {folder.Value}");
            return false;
        }

        audioClipGUIDs = AssetDatabase.FindAssets("t:AudioClip", new[] { MusicFolder, SfxFolder });
        EnsureAudioDataFolderExists();
        return true;
    }

    private void EnsureAudioDataFolderExists()
    {
        if (Directory.Exists(_audioDataFolder)) return;
        
        Directory.CreateDirectory(_audioDataFolder);
        AssetDatabase.Refresh();
    }

    [ContextMenu("Create AudioData Assets")]
    public void CreateAudioDataAssets()
    {
        if (!LoadAudioClips()) return; // Can't create audio assets if fail to load audio clip

        foreach (string guid in audioClipGUIDs)
        {
            string clipPath = AssetDatabase.GUIDToAssetPath(guid);
            AudioClip clip = AssetDatabase.LoadAssetAtPath<AudioClip>(clipPath);

            string audioDataPath = Path.Combine(_audioDataFolder, $"{clip.name}.asset");

            if (AssetDatabase.LoadAssetAtPath<AudioData>(audioDataPath) != null) continue;

            AudioData audioData = ScriptableObject.CreateInstance<AudioData>();
            audioData.audioClip = clip;
            audioData.name = clip.name;

            if (clipPath.StartsWith(MusicFolder))
                audioData.audioType = AudioClipType.BACKGROUND;
            else if (clipPath.StartsWith(SfxFolder))
                audioData.audioType = AudioClipType.SFX;

            // Create the asset in the specified folder
            AssetDatabase.CreateAsset(audioData, audioDataPath);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("AudioData assets created successfully!");
    }
}
