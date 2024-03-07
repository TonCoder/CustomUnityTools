using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CVStudio
{
    [CreateAssetMenu(fileName = "New Music List", menuName = "CVeinStudio/Game System/Audio/MusicList")]
    public class SO_MusicList : ScriptableObject
    {
        [SerializeField] internal List<MusicInfo> songList;
    }

    [System.Serializable]
    public struct MusicInfo
    {
        [SerializeField] internal string _songName;
        [SerializeField] internal AudioClip _audioClip;
    }
}