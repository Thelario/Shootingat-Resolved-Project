using UnityEngine;
using System.Collections.Generic;

namespace PabloLario.Managers
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundManager : Singleton<SoundManager>
    {
        // Volume of SFX
        [SerializeField] private float defaultPitch = 1f;
        [SerializeField] private float pitchRandomModifier = 0.1f;

        [SerializeField] private AudioSource sfxSource;
        [SerializeField] private AudioSource musicAudioSource;

        private Dictionary<SoundType, float> _soundTimerDictionary;

        protected override void Awake()
        {
            base.Awake();

            Initialize();
        }

        private void Initialize()
        {
            _soundTimerDictionary = new Dictionary<SoundType, float>
            {
                [SoundType.PlayerWalk] = 0f
            };
        }

        public void ChangeMusicVolume()
        {
            musicAudioSource.volume = OptionsManager.Instance.MasterVolume * OptionsManager.Instance.MusicVolume * OptionsManager.Instance.MusicVolume;
            if (musicAudioSource.volume < 0.001)
                musicAudioSource.volume = 0f;
        }

        public void PlaySound(SoundType st)
        {
            if (CanPlaySound(st))
            {
                sfxSource.pitch = Random.Range(defaultPitch - pitchRandomModifier, defaultPitch + pitchRandomModifier);
                sfxSource.PlayOneShot(SearchSound(st), OptionsManager.Instance.MasterVolume * OptionsManager.Instance.SfxVolume);
            }
        }

        public void PlaySound(SoundType st, float newVolume)
        {
            if (CanPlaySound(st))
            {
                sfxSource.pitch = Random.Range(defaultPitch - pitchRandomModifier, defaultPitch + pitchRandomModifier);
                sfxSource.PlayOneShot(SearchSound(st), OptionsManager.Instance.MasterVolume * OptionsManager.Instance.SfxVolume * newVolume);
            }
        }

        private bool CanPlaySound(SoundType sound)
        {
            switch (sound)
            {
                default:
                    return true;
                case SoundType.PlayerWalk:
                    if (_soundTimerDictionary.ContainsKey(sound))
                    {
                        float lastTimePlayed = _soundTimerDictionary[sound];
                        float playerMoveTimerMax = .25f;
                        if (lastTimePlayed + playerMoveTimerMax < Time.time)
                        {
                            _soundTimerDictionary[sound] = Time.time;
                            return true;
                        }
                        else
                            return false;
                    }
                    else
                        return true;
            }
        }

        private AudioClip SearchSound(SoundType st)
        {
            /* CODE FOR LOOPING THROUGH LIST
            foreach (SoundAudioClip sac in Assets.Instance.soundAudioClipArray)
            {
                if (sac.sound == st)
                    return sac.audioClip;
            }
            */
            // CODE FOR SEARCHING IN DICTIONARY
            Assets.Instance.soundAudioClipDictionary.TryGetValue(st, out AudioClip clip);

            if (clip != null)
                return clip;
            else
            {
                Debug.LogError("Sound Not Found");
                return null;
            }
        }
    }
}