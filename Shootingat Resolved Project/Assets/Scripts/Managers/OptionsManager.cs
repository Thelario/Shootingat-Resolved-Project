using System;
using UnityEngine;

namespace PabloLario.Managers
{
    public class OptionsManager : Singleton<OptionsManager>
    {
        private PostProcessingManager ppm;

        private void Start()
        {
            ppm = PostProcessingManager.Instance;
            
            SetOptionsValues();
        }

        private void SetOptionsValues()
        {
            // TODO: here I need to add the logic for the save & load that is related to the sacing of the player
            // preferences of the options, so that they don't change every time the player restarts the game.

            MasterVolume = 1f;
            MusicVolume = 1f;
            SfxVolume = 1f;
            BloomEffect = true;
            GrainEffect = true;
        }

        private float masterVolume;
        public float MasterVolume
        {
            get => masterVolume;
            set
            {
                SoundManager.Instance.ChangeMusicVolume();
                masterVolume = Mathf.Clamp(value, 0f, 1f);
            }
        }

        private float musicVolume;
        public float MusicVolume
        {
            get => musicVolume;
            set
            {
                SoundManager.Instance.ChangeMusicVolume();
                musicVolume = Mathf.Clamp(value, 0f, 1f);
            }
        }

        private float sfxVolume;
        public float SfxVolume
        {
            get => sfxVolume;
            set => sfxVolume = Mathf.Clamp(value, 0f, 1f);
        }

        private bool bloomEffect;
        public bool BloomEffect
        {
            get => bloomEffect;
            set
            {
                bloomEffect = value;
                ppm.SetBloom(value);
            }
        }

        private bool grainEffect;
        public bool GrainEffect
        {
            get => grainEffect;
            set
            {
                grainEffect = value;
                ppm.SetGrain(value);
            }
        }
    }
}