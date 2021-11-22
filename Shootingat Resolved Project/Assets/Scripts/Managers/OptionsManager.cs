using UnityEngine;
using UnityEngine.Rendering;

namespace PabloLario.Managers
{
    public class OptionsManager : Singleton<OptionsManager>
    {
        [SerializeField] private VolumeProfile bloomVolumeProfile;
        
        private float masterVolume;
        public float MasterVolume
        {
            get => masterVolume;
            set
            {
                // Here I am probably gonna have to add something to
                // change the current volume of the music
                masterVolume = Mathf.Clamp(value, 0f, 1f);
            }
        }

        private float musicVolume;
        public float MusicVolume
        {
            get => musicVolume;
            set
            {
                // Here I am probably gonna have to add something to
                // change the current volume of the music
                musicVolume = Mathf.Clamp(value, 0f, 1f);
            }
        }

        private float sfxVolume;
        public float SfxVolume
        {
            get => sfxVolume;
            set
            {
                sfxVolume = Mathf.Clamp(value, 0f, 1f);
            }
        }

        private bool bloomEffect;
        public bool BloomEffect
        {
            get => bloomEffect;
            set
            {
                Debug.Log("UO" + value);
            }
        }

        private bool grainEffect;
        public bool GrainEffect
        {
            get => grainEffect;
            set
            {
                Debug.Log("UO" + value);
            }
        }
    }
}