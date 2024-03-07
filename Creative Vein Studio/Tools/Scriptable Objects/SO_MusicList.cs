using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace MAIN_PROJECT._Scripts.Tools.Scriptable_Objects
{
    [CreateAssetMenu(fileName = "New Music List", menuName = "CVeinStudio/Game System/Audio/MusicList")]
    public class SO_MusicList : ScriptableObject
    {
        [SerializeField] internal List<MusicInfo> collection;
    }

    [System.Serializable]
    public struct MusicInfo
    {
        [SerializeField] internal string _sceneName;
        [SerializeField] internal AudioClip[] _audioClip;
    }
}