using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PabloLario.Managers
{
    public class OptionsManager : Singleton<OptionsManager>
    {
        private float masterVolume;
        public float MasterVolume
        {
            get => masterVolume;
            set => masterVolume = Mathf.Clamp(value, 0f, 1f);
        }

        private float musicVolume;
        public float MusicVolume
        {
            get => musicVolume;
            set => musicVolume = Mathf.Clamp(value, 0f, 1f);
        }

        private float sfxVolume;
        public float SfxVolume
        {
            get => sfxVolume;
            set => sfxVolume = Mathf.Clamp(value, 0f, 1f);
        }

        public bool BloomEffect { get; set; }
        public bool GrainEffect { get; set; }
    }
}