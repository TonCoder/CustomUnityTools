using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CVStudio
{
    public class SoundFXManager : MonoBehaviour
    {

        [Space(5), Header("Sound Fx")]
        public AudioSource asource;
        public SO_SoundFX _dashFx;

        public void PlayDashFx()
        {
            //_dashFx.PlayRandomSoundFx(asource);
        }


    }
}