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
        
        private void Start()
        {
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
            masterVolumeSlider.value = OptionsManager.Instance.MasterVolume;
            musicVolumeSlider.value = OptionsManager.Instance.MusicVolume;
            sfxVolumeSlider.value = OptionsManager.Instance.SfxVolume;
            bloomToggle.isOn = OptionsManager.Instance.BloomEffect;
            grainToggle.isOn = OptionsManager.Instance.GrainEffect;
        }
        
        private void SetMasterVolume(float vol) { OptionsManager.Instance.MasterVolume = vol; }
        private void SetMusicVolume(float vol) { OptionsManager.Instance.MusicVolume = vol; }
        private void SetSfxVolume(float vol) { OptionsManager.Instance.SfxVolume = vol; }
        private void SetBloomEffect(bool on) { OptionsManager.Instance.BloomEffect = on; }
        private void SetGrainEffect(bool on) { OptionsManager.Instance.GrainEffect = on; }
    }
}
