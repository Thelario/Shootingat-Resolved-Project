using System;
using UnityEngine;

namespace PabloLario.Managers
{
    public class OptionsManager : Singleton<OptionsManager>
    {
        private PostProcessingManager _ppm;

        private void Start()
        {
            _ppm = PostProcessingManager.Instance;
            
            SetOptionsValues();
        }

        private void SetOptionsValues()
        {
            // TODO: here I need to add the logic for the save & load that is related to the saving of the player
            // preferences of the options, so that they don't change every time the player restarts the game.

            MasterVolume = .5f;
            MusicVolume = 0f;
            SfxVolume = 1f;
            BloomEffect = true;
            GrainEffect = true;
        }

        private float _masterVolume;
        public float MasterVolume
        {
            get => _masterVolume;
            set
            {
                _masterVolume = Mathf.Clamp(value, 0f, 1f);
                SoundManager.Instance.ChangeMusicVolume();
            }
        }

        private float _musicVolume;
        public float MusicVolume
        {
            get => _musicVolume;
            set
            {
                _musicVolume = Mathf.Clamp(value, 0f, 1f);
                SoundManager.Instance.ChangeMusicVolume();
            }
        }

        private float _sfxVolume;
        public float SfxVolume
        {
            get => _sfxVolume;
            set => _sfxVolume = Mathf.Clamp(value, 0f, 1f);
        }

        private bool _bloomEffect;
        public bool BloomEffect
        {
            get => _bloomEffect;
            set
            {
                _bloomEffect = value;
                _ppm.SetBloom(value);
            }
        }

        private bool _grainEffect;
        public bool GrainEffect
        {
            get => _grainEffect;
            set
            {
                _grainEffect = value;
                _ppm.SetGrain(value);
            }
        }
    }
}