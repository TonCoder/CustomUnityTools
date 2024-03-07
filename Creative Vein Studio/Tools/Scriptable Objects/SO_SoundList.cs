using System;
using System.Collections;
using System.Collections.Generic;
using CVStudio;
using UnityEngine;

[CreateAssetMenu(fileName = "_SFxList", menuName = "CVeinStudio/Game System/Audio/SoundList")]
public class SO_SoundList : ScriptableObject
{
    [SerializeField] internal List<AudioInfo> collection = new List<AudioInfo>();
    internal Dictionary<SO_Enum, AudioInfo> audioCollection = new Dictionary<SO_Enum, AudioInfo>();

    private void OnEnable()
    {
        audioCollection.Clear();
        for (var i = 0; i < collection.Count; i++)
        {
            audioCollection.Add(collection[i].SoEnumerator, collection[i]);
        }
    }
}

[System.Serializable]
public class AudioInfo
{
    [SerializeField] private SO_Enum soEnum;
    [SerializeField] private List<AudioClip> soundClip = new List<AudioClip>();
    [SerializeField, Range(0f, 1f)] private float soundVolume = 0.9f;

    [SerializeField] private Vector2 pitch = Vector2.one;

    public float Volume => soundVolume;
    public Vector2 Pitch => pitch;
    public List<AudioClip> Clips => soundClip;
    public SO_Enum SoEnumerator => soEnum;
}