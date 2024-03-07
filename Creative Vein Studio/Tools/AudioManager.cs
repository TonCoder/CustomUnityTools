using System;
using System.Collections;
using System.Linq;
using CreativeVeinStudio.Simple_Dialogue_System.Attributes;
using CVStudio;
using MAIN_PROJECT._Scripts.Enums;
using MAIN_PROJECT._Scripts.Tools.Scriptable_Objects;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace MAIN_PROJECT._Scripts.Tools
{
    public class AudioManager : MonoBehaviour
    {
        public bool makeSingleton = false;
        public static AudioManager Instance;

        [FieldTitle("Button Audio Clips")] [SerializeField]
        private AudioClip buttonClick_success;

        [SerializeField] private AudioClip buttonClick_Cancel;


        [FieldTitle("Audio List")] 
        [SerializeField] private SO_MusicList _musicList;
        [SerializeField] private SO_SoundList _soundList;

        [FieldTitle("Audio Setup")] 
        [SerializeField] private AudioMixer mixer;

        [SerializeField] private AudioMixerGroup musicGroup;
        [SerializeField] private AudioMixerGroup fxGroup;
        [SerializeField] private AudioMixerGroup UiGroup;

        [FieldTitle("Audio Settings")] 
        [SerializeField] private float transitionTime = 0.5f;

        [FieldTitle("Audio GameObjects")] 
        [SerializeField] private AudioSource asMusic;

        [SerializeField] private AudioSource asSfx;
        [SerializeField] private AudioSource asUi;

        private float _musicAudioLvl = 1;

        private AudioClip? _transitionToClip;

        private void Awake()
        {
            if (makeSingleton) Singleton();

            asMusic ??= gameObject.AddComponent<AudioSource>();
            asSfx ??= gameObject.AddComponent<AudioSource>();
            asUi ??= gameObject.AddComponent<AudioSource>();

            //Init
            asMusic.outputAudioMixerGroup = musicGroup;
            asSfx.outputAudioMixerGroup = fxGroup;
            asUi.outputAudioMixerGroup = UiGroup;

            asMusic.loop = true;
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += SetMusicByScene;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= SetMusicByScene;
        }

        private void SetMusicByScene(Scene scene, LoadSceneMode mode)
        {
#if UNITY_EDITOR
            Debug.LogWarning("Set Music by scene will always play the first audio ONLY");
#endif
            _transitionToClip = _musicList.collection?
                .FirstOrDefault(x => x._sceneName == scene.name)._audioClip?.First();

            if (_transitionToClip == null)
            {
                return;
            }

            if (asMusic.isPlaying)
            {
                _musicAudioLvl = asMusic.volume;
                StartCoroutine(TransitionAudio());
                return;
            }

            asMusic.clip = _transitionToClip;
            asMusic.Play();
        }

        public void UpdateVolume(EAudioChannel channel, float volLevel)
        {
            mixer.SetFloat(channel.ToString(), volLevel);
        }

        public void PlayButtonSuccess()
        {
            if (buttonClick_success == null)
            {
                Debug.LogWarning("Needs Button success audio clip");
                return;
            }

            asUi.clip = buttonClick_success;
            asUi.Play();
        }

        public void PlayButtonCancel()
        {
            if (buttonClick_Cancel == null)
            {
                Debug.LogWarning("Needs Button cancel audio clip");
                return;
            }

            asUi.clip = buttonClick_Cancel;
            asUi.Play();
        }

        public void PlaySfx(SO_Enum enumToPlay)
        {
            if (_soundList != null && _soundList.collection.Count > 0)
            {
                var info = _soundList.audioCollection[enumToPlay];
                asSfx.clip = info.Clips[Random.Range(0, info.Clips.Count - 1)];
                asSfx.volume = info.Volume;
                asSfx.pitch = Random.Range(info.Pitch.x, info.Pitch.y);
                asSfx.Play();
            }
        }

        public void PlayAudio(AudioClip clip, EAudioChannel channel)
        {
            switch (channel)
            {
                case EAudioChannel.Music:
                    asMusic.clip = clip;
                    asMusic.Play();
                    break;
                case EAudioChannel.Fx:
                    asSfx.clip = clip;
                    asSfx.Play();
                    break;
                case EAudioChannel.Ui:
                    asUi.clip = clip;
                    asUi.Play();
                    break;
                default:
                    break;
            }
        }

        IEnumerator TransitionAudio()
        {
            while (_musicAudioLvl > 0)
            {
                _musicAudioLvl--;
                yield return new WaitForSeconds(transitionTime);
                asMusic.volume = _musicAudioLvl;
            }

            asMusic.clip = _transitionToClip;
            asMusic.Play();

            while (_musicAudioLvl <= 0)
            {
                _musicAudioLvl++;
                yield return new WaitForSeconds(transitionTime);
                asMusic.volume = _musicAudioLvl;
            }

            yield return 0;
        }

        private void Singleton()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }
    }
}