using System.Linq;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(AudioDataFactory))]
public class AudioManger : MonoBehaviour
{
    private AudioDataFactory _audioDataFactory;
    private AudioData[] _audioDataList;

    private void Awake()
    {
        _audioDataFactory = GetComponent<AudioDataFactory>();
        _audioDataFactory.CreateAudioDataAssets();

        Object[] _tempList = AssetDatabase.LoadAllAssetsAtPath(_audioDataFactory.AudioDataFolder);
        _audioDataList = _tempList.OfType<AudioData>().ToArray();
    }
}
