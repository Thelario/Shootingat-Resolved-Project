using PabloLario.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class OptionsBehaviour : MonoBehaviour
    {
        [SerializeField] private Slider masterVolumeSlider;
        [SerializeField] private Slider musicVolumeSlider;
        [SerializeField] private Slider sfxVolumeSlider;
        [SerializeField] private Toggle bloomToggle;
        [SerializeField] private Toggle grainToggle;

        private OptionsManager options;
        
        private void Start()
        {
            options = OptionsManager.Instance;
            
            SetSettings();
        }

        private void OnEnable()
        {
            SetValuesFromOptions();
        }

        private void SetSettings()
        {
            masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
            musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
            sfxVolumeSlider.onValueChanged.AddListener(SetSfxVolume);
            bloomToggle.onValueChanged.AddListener(SetBloomEffect);
            grainToggle.onValueChanged.AddListener(SetGrainEffect);
        }

        private void SetValuesFromOptions()
        {
            masterVolumeSlider.value = options.MasterVolume;
            musicVolumeSlider.value = options.MusicVolume;
            sfxVolumeSlider.value = options.SfxVolume;
            bloomToggle.isOn = options.BloomEffect;
            grainToggle.isOn = options.GrainEffect;
        }
        
        private void SetMasterVolume(float vol) { options.MasterVolume = vol; }
        private void SetMusicVolume(float vol) { options.MusicVolume = vol; }
        private void SetSfxVolume(float vol) { options.SfxVolume = vol; }
        private void SetBloomEffect(bool on) { options.BloomEffect = on; }
        private void SetGrainEffect(bool on) { options.GrainEffect = on; }
    }
}
