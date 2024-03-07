using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSoundFx", menuName = "CVeinStudio/Game System/Audio/SoundFx")]
public class SO_SoundFX : ScriptableObject
{
    [SerializeField, Range(0f, 1f)] private float _soundVolume;
    [SerializeField] private Vector2 _pitch;
    [SerializeField] private List<AudioClip> _soundClip;

    public float Volume { get { return _soundVolume; } }
    public Vector2 Pitch{ get { return _pitch; }  }
    public List<AudioClip> Clip { get { return _soundClip; } }

    void PlayTest()
    {
        AudioSource.PlayClipAtPoint(Clip[Random.Range(0, _soundClip.Count - 1)], Vector3.zero, Volume);
    }

}
