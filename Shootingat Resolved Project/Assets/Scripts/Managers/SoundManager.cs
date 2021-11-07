using UnityEngine;
using System.Collections.Generic;

namespace PabloLario.Managers
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundManager : Singleton<SoundManager>
    {
        [SerializeField] private float volume; // Volume of SFX
        [SerializeField] private float defaultPitch = 1f;
        [SerializeField] private float pitchRandomModifier = 0.1f;

        private AudioSource source; // Private reference of the audioSource where we are going to play our SFX

        private Dictionary<SoundType, float> soundTimerDictionary;

        protected override void Awake()
        {
            base.Awake();

            source = GetComponent<AudioSource>();

            Initialize();
        }

        private void Initialize()
        {
            soundTimerDictionary = new Dictionary<SoundType, float>
            {
                [SoundType.PlayerWalk] = 0f
            };
        }

        public void PlaySound(SoundType st)
        {
            if (CanPlaySound(st))
            {
                source.pitch = Random.Range(defaultPitch - pitchRandomModifier, defaultPitch + pitchRandomModifier);
                source.PlayOneShot(SearchSound(st), volume * volume);
            }
        }

        public void PlaySound(SoundType st, float newVolume)
        {
            if (CanPlaySound(st))
            {
                source.pitch = Random.Range(defaultPitch - pitchRandomModifier, defaultPitch + pitchRandomModifier);
                source.PlayOneShot(SearchSound(st), volume * newVolume);
            }
        }

        private bool CanPlaySound(SoundType sound)
        {
            switch (sound)
            {
                default:
                    return true;
                case SoundType.PlayerWalk:
                    if (soundTimerDictionary.ContainsKey(sound))
                    {
                        float lastTimePlayed = soundTimerDictionary[sound];
                        float playerMoveTimerMax = .475f;
                        if (lastTimePlayed + playerMoveTimerMax < Time.time)
                        {
                            soundTimerDictionary[sound] = Time.time;
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
                Debug.LogError("Sound Not Found");
                return null;
        }
    }
}