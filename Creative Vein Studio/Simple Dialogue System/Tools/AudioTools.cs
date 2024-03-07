using System;
using System.Collections;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CreativeVeinStudio.Simple_Dialogue_System.Tools
{
    public class AudioTools
    {
        private static AudioSource _aSource;

        public static Texture2D PaintWaveformSpectrum(float[] waveform, int height, Color c)
        {
            if (waveform.Length <= 0) return Texture2D.grayTexture;

            Texture2D tex = new Texture2D(waveform.Length, height, TextureFormat.RGB24, false);

            for (int x = 0; x < waveform.Length; x++)
            {
                for (int y = 0; y <= waveform[x] * (float)height / 2f; y++)
                {
                    tex.SetPixel(x, (height / 2) + y, c);
                    tex.SetPixel(x, (height / 2) - y, c);
                }
            }

            tex.Apply();

            return tex;
        }

        public static float[] GetWaveform(AudioClip audio, int size, float sat)
        {
            if (audio == null) return Array.Empty<float>();

            float[] samples = new float[audio.channels * audio.samples];
            float[] waveform = new float[size];
            audio.GetData(samples, 0);
            int packSize = audio.samples * audio.channels / size;
            float max = 0f;
            int c = 0;
            int s = 0;
            for (int i = 0; i < audio.channels * audio.samples; i++)
            {
                waveform[c] += Mathf.Abs(samples[i]);
                s++;
                if (s > packSize)
                {
                    if (max < waveform[c])
                        max = waveform[c];
                    c++;
                    s = 0;
                }
            }

            for (int i = 0; i < size; i++)
            {
                waveform[i] /= (max * sat);
                if (waveform[i] > 1f)
                    waveform[i] = 1f;
            }

            return waveform;
        }

        public static void TestAudio(AudioClip clip, [UnityEngine.Internal.DefaultValue("1.0F")] float volume)
        {
            GameObject gameObject = new GameObject("One shot audio");
            gameObject.transform.position = Vector3.zero;
            _aSource = (AudioSource)gameObject.AddComponent(typeof(AudioSource));
            _aSource.clip = clip;
            _aSource.spatialBlend = 1f;
            _aSource.volume = volume;
            _aSource.Play();

            // EditorCoroutineUtility.StartCoroutine(KillAudio(clip, gameObject), gameObject);
        }

        public static void StopAudioClip()
        {
            if (_aSource)
            {
                _aSource.Stop();
            }
            else
            {
                Debug.LogWarning("No audio playing");
            }
        }

        static IEnumerator KillAudio(AudioClip clip, GameObject gameObject)
        {
            // yield return new EditorWaitForSeconds(10);

            // yield return new EditorWaitForSeconds(clip.length *
            //                                       ((double)Time.timeScale < 0.009999999776482582
            //                                           ? 0.01f
            //                                           : Time.timeScale));

            _aSource = null;
            Object.DestroyImmediate(gameObject, true);
            yield return 0;
        }
    }
}