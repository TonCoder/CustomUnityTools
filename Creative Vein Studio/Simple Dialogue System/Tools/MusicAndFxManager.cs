using CVStudio;
using System;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

/// <summary>
/// This script takes advantage of Unity's Audio Mixer to best manage your audio settings in game.
/// Create volume variables for the mixers. follow the link "https://www.youtube.com/watch?v=7wWNAiWc8ws&feature=emb_title&ab_channel=Unity"
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class MusicAndFxManager : MonoBehaviour
{
    public static MusicAndFxManager instance;

    [Header("General Settings")] [SerializeField]
    private SO_GeneralSettings gameSettings;

    [SerializeField] private TMPro.TextMeshProUGUI _musicNameText;
    [SerializeField] private bool _loadFirstSong;

    [Header("Audio Mixer Setup")]
    // [SerializeField] private AudioMixer mixer;
    [SerializeField]
    private AudioMixerGroup masterMixer;

    [SerializeField] private AudioMixerGroup musicMixer;
    [SerializeField] private AudioMixerGroup sfxMixer;
    [SerializeField] private AudioMixerGroup uiMixer;

    [Header("Audio Mixer Setup")] [SerializeField]
    private string masterVariableName = "MasterVolume";

    [SerializeField] private string musicVariableName = "MusicVol";
    [SerializeField] private string fxVariableName = "FXVol";
    [SerializeField] private string uiVariableName = "UIVol";

    [Header("Scene Music Info")] [SerializeField]
    private SO_MusicList _music;

    [Header("Audio Setup")] [SerializeField]
    private AudioSource musicASource;

    [SerializeField] private AudioSource sfxASource;
    [SerializeField] private AudioSource uiASource;

    // [Header("Snapshot Setup")]
    // [SerializeField] private AudioMixerSnapshot UnMuteSnapshot;
    // [SerializeField] private AudioMixerSnapshot MuteSnapshot; 

    private MusicInfo _activeMusicPlaying;

    private void Start()
    {
        if (gameSettings == null)
        {
            Debug.Log("The SO_GeneralSettings has not been set in MusicAndFxManager");
            Debug.Break();
        }

        if (_loadFirstSong && _music.songList.Count > 0)
        {
            _activeMusicPlaying = _music.songList[0];
            musicASource.clip = _activeMusicPlaying._audioClip;
            if (_musicNameText != null) _musicNameText.text = _activeMusicPlaying._audioClip.name;
        }

        musicASource.outputAudioMixerGroup = musicMixer;
        sfxASource.outputAudioMixerGroup = sfxMixer;
        uiASource.outputAudioMixerGroup = uiMixer;
        
        SoundSettingsLoaded(gameSettings);
        Singleton();
    }

    public void SoundSettingsLoaded(SO_GeneralSettings gameSoundSettings)
    {
        masterMixer.audioMixer.SetFloat(masterVariableName, gameSoundSettings.masterVol);
        masterMixer.audioMixer.SetFloat(musicVariableName, gameSoundSettings.musicVol);
        masterMixer.audioMixer.SetFloat(fxVariableName, gameSoundSettings.fxVol);
        masterMixer.audioMixer.SetFloat(uiVariableName, gameSoundSettings.uiVol);
    }

    // *****************************
    // PLAY CONTROLS - 
    // *****************************

    #region Play controls

    public void PlayFx(AudioClip clip)
    {
        sfxASource.clip = clip;
        sfxASource.Play();
    }

    public void PlayUi(AudioClip clip)
    {
        uiASource.clip = clip;
        uiASource.Play();
    }

    public void PlayMusic(AudioClip clip)
    {
        musicASource.clip = clip;
        musicASource.Play();
    }

    public void TogglePlay(bool val)
    {
        if (val) musicASource.Pause();
        else musicASource.Play();
    }

    public void PlayNextPrev(bool isNext)
    {
        if (_music == null) return;
        
        var index = isNext
            ? _music.songList.IndexOf(_activeMusicPlaying) + 1
            : _music.songList.IndexOf(_activeMusicPlaying) - 1;
        if (index >= _music.songList.Count)
        {
            index = 0;
        }
        else if (index < 0)
        {
            index = _music.songList.Count - 1;
        }

        _activeMusicPlaying = _music.songList[index];

        if (_musicNameText != null) _musicNameText.text = _activeMusicPlaying._audioClip.name;
        musicASource.clip = _activeMusicPlaying._audioClip;
        musicASource.Play();
    }

    public void PlayMusiFromList(string name)
    {
        if (_music == null) return;

        _activeMusicPlaying = _music.songList.FirstOrDefault(x => x._songName.Equals(name));
        if (_musicNameText != null) _musicNameText.text = _activeMusicPlaying._audioClip.name;
        musicASource.clip = _activeMusicPlaying._audioClip;
        musicASource.Play();
    }

    #endregion

    // *****************************************
    // AUDIO CONTROLS - VOLUME, TOGGLES
    // *****************************************

    #region Audio Controls

    public void SetMasterMusicLevel(float volume)
    {
        masterMixer.audioMixer.SetFloat(masterVariableName, volume);
        gameSettings.masterVol = (int) volume;
    }

    public void SetMusicLevel(float volume)
    {
        masterMixer.audioMixer.SetFloat(musicVariableName, volume);
        gameSettings.musicVol = (int) volume;
    }

    public void SetFXLevel(float volume)
    {
        masterMixer.audioMixer.SetFloat(fxVariableName, volume);
        gameSettings.fxVol = (int) volume;
    }

    public void SetUILevel(float volume)
    {
        masterMixer.audioMixer.SetFloat(uiVariableName, volume);
        gameSettings.uiVol = (int) volume;
    }

    public void ToggleMute(bool val)
    {
        if (val)
        {
            // UnMuteSnapshot.TransitionTo(0.5f);
            SetMasterMusicLevel(gameSettings.masterVol);
        }
        else
        {
            // MuteSnapshot.TransitionTo(0.5f);
            masterMixer.audioMixer.SetFloat(masterVariableName, -80);
        }
    }

    public void ToggleMusicMute(bool val)
    {
        if (val)
        {
            SetMusicLevel(gameSettings.musicVol);
        }
        else
        {
            masterMixer.audioMixer.SetFloat(musicVariableName, -80);
        }
    }

    public void ToggleSfxMute(bool val)
    {
        if (val)
        {
            SetFXLevel(gameSettings.fxVol);
        }
        else
        {
            masterMixer.audioMixer.SetFloat(fxVariableName, -80);
        }
    }

    public void ToggleUiMute(bool val)
    {
        if (val)
        {
            SetUILevel(gameSettings.uiVol);
        }
        else
        {
            masterMixer.audioMixer.SetFloat(uiVariableName, -80);
        }
    }

    #endregion


    public void LevelFinishedLoading(Scene scene)
    {
        try
        {
            for (int i = 0; i < _music.songList.Count; i++)
            {
                if (_music.songList[i]._songName == scene.name &&
                    _music.songList[i]._audioClip != _activeMusicPlaying._audioClip)
                {
                    _activeMusicPlaying = _music.songList[i];
                    musicASource.clip = _activeMusicPlaying._audioClip;
                    musicASource.loop = true;
                    musicASource.Play();
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log($"Error loading Music: {ex.Message}");
        }
    }

    private void Singleton()
    {
        if (instance == null || instance != this)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}