using System;
using System.Collections.Generic;
using UnityEngine;

namespace CVStudio
{
    [CreateAssetMenu(menuName = "CVeinStudio/Settings/GameSettings", fileName = "Game Settings")]
    public class SO_GeneralSettings : ScriptableObject
    {
        [SerializeField, Range(-80, 0)] internal int masterVol = 0;
        [SerializeField, Range(-80, 0)] internal int musicVol = 0;
        [SerializeField, Range(-80, 0)] internal int fxVol = 0;
        [SerializeField, Range(-80, 0)] internal int uiVol = 0;
        
        [SerializeField] internal string dateTimeVal = DateTime.Now.ToString("HHmmss");
        [SerializeField] internal List<GameScore> gameScore = new List<GameScore>();
    }

    [System.Serializable]
    public class GameScore
    {
        public string name;
        public int score;
    }
}