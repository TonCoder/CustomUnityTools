using UnityEngine;

namespace HeistEscape
{
    [System.Serializable]
    public class GamePrefs : MonoBehaviour
    {
        private const string _MUSICLEVEL = "Music_Level";
        private const string _SOUNDLEVEL = "Sound_Level";
        private const string _DIFFICULTY = "Difficulty";
        private const string _PLAYERSCORE = "Player_Score";
        private const string _ENEMIESKILLED = "Enemies_Killed";
        private const string _ASTEROIDSDES = "Asteroid_Destroyed";
        private const string _TIMETAKEN = "Time_Taken";
        private const string _PLAYERGEMS = "PGms";

        //------------------------------------
        //  Setters
        //------------------------------------

        public static void SetTimeTaken(string value, bool isFromClass)
        {
            if (isFromClass)
                PlayerPrefs.SetString(_TIMETAKEN, value);
        }

        public static void SetAsteroidDestroyed(float value, bool isFromClass)
        {
            if (isFromClass)
                PlayerPrefs.SetFloat(_ASTEROIDSDES, value);
        }

        public static void SetEnemiesKilled(float value, bool isFromClass)
        {
            if (isFromClass)
                PlayerPrefs.SetFloat(_ENEMIESKILLED, value);
        }

        public static void SetPlayerGms(float value, bool isFromClass)
        {
            if (isFromClass)
                PlayerPrefs.SetFloat(_PLAYERGEMS, value);
        }

        public static void SetSoundLevel(float volume)
        {
            if (volume < 0f || volume > 1f)
            {
                Debug.LogError("The Sound Volume is out of bound, unable to set specified value: " + volume);
                return;
            }

            Debug.Log(volume);
            PlayerPrefs.SetFloat(_SOUNDLEVEL, volume);
        }

        public static void SetMusicLevel(float value)
        {
            if (value < 0f || value > 1f)
            {
                Debug.Log("Unable to set " + value + " On music level");
                return;
            }

            PlayerPrefs.SetFloat(_MUSICLEVEL, value);
        }

        public static void SetScore(int score)
        {
            if (score < 0)
            {
                Debug.Log("Unable to set value less than 0");
                return;
            }

            PlayerPrefs.SetInt(_PLAYERSCORE, score);
        }

        //------------------------------------
        // Getters
        //------------------------------------
        public static float GetGems()
        {
            return PlayerPrefs.GetFloat(_PLAYERGEMS, 1f);
        }

        public static float GetSoundLevel()
        {
            return PlayerPrefs.GetFloat(_SOUNDLEVEL, 1f);
        }

        public static float GetMusicLevel()
        {
            return PlayerPrefs.GetFloat(_MUSICLEVEL, 1f);
        }

        public static int GetDifficulty()
        {
            return PlayerPrefs.GetInt(_DIFFICULTY, 0);
        }

        public static int GetScore()
        {
            return PlayerPrefs.GetInt(_PLAYERSCORE, 0000);
        }

        //------------------------------------
        // Saving changes
        //------------------------------------
        public static void SaveGamePrefChanges()
        {
            PlayerPrefs.Save();
        }
    }
}