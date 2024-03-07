using System;
using UnityEngine;
using UnityEngine.Audio;

namespace CVStudio
{
    [CreateAssetMenu(menuName = "CVeinStudio/Settings/Audio Settings", fileName = "Audio Settings")]
    public class SO_AudioSettings : ScriptableObject
    {
        #region Properties

        public int MasterVol
        {
            get => masterVol;
            set => masterVol = value;
        }

        public int MusicVol
        {
            get => musicVol;
            set => musicVol = value;
        }

        public int FxVol
        {
            get => fxVol;
            set => fxVol = value;
        }

        public int UiVol
        {
            get => uiVol;
            set => uiVol = value;
        }

        public AudioMixerGroup MasterMixer => masterMixer;
        public AudioMixerGroup MusicMixer => musicMixer;
        public AudioMixerGroup FxMixer => fxMixer;
        public AudioMixerGroup UiMixer => uiMixer;

        public AudioMixerSnapshot DefaultSnapshot => startingSnapshot;
        public AudioMixerSnapshot MuteSnapshot => mutingSnapshot;

        #endregion

        #region Exposed Vars

        [Header("Start settings")] [SerializeField]
        internal int masterVol = 20;

        [SerializeField] private int musicVol = 20;

        [SerializeField] private int fxVol = 20;

        [SerializeField] private int uiVol = 20;

        [Header("Audio Mixer Setup")] [SerializeField]
        private AudioMixerGroup masterMixer;

        [SerializeField] private AudioMixerGroup musicMixer;
        [SerializeField] private AudioMixerGroup fxMixer;
        [SerializeField] private AudioMixerGroup uiMixer;


        [Header("Snapshot Setup")] [SerializeField]
        private AudioMixerSnapshot startingSnapshot;

        [SerializeField] private AudioMixerSnapshot mutingSnapshot;

        #endregion

        #region Private Vars

        public const string MusicVariableName = "MusicVolume";
        public const string MasterVariableName = "MasterVolume";
        public const string FXVariableName = "SfxVolume";
        public const string UIVariableName = "UiVolume";
        private const string PrefsName = "VolumeSettings";

        #endregion

        public void Reset()
        {
            masterVol = musicVol = fxVol = uiVol = 20;
        }

        /// <summary>
        /// Saves settings to PlayerPrefs
        /// </summary>
        public void SaveSettings()
        {
            PlayerPrefs.SetString(PrefsName, $"{masterVol},{musicVol},{fxVol},{uiVol}");
            PlayerPrefs.Save();
        }

        /// <summary>
        /// Load settings to PlayerPrefs
        /// </summary>
        public void LoadSettings()
        {
            var prefVals = PlayerPrefs.GetString(PrefsName)?.Split(',');
            if (!(prefVals?.Length > 0)) return;
            masterVol = int.Parse(prefVals[0]);
            musicVol = int.Parse(prefVals[1]);
            fxVol = int.Parse(prefVals[2]);
            uiVol = int.Parse(prefVals[3]);
        }
    }
}