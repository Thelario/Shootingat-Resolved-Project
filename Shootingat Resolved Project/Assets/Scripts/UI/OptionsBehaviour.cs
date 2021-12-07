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
        [SerializeField] private Slider bloomSlider;
        [SerializeField] private Slider grainSlider;
        [SerializeField] private Slider cameraShakeSlider;
        
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
            bloomSlider.onValueChanged.AddListener(SetBloomEffect);
            grainSlider.onValueChanged.AddListener(SetGrainEffect);
            cameraShakeSlider.onValueChanged.AddListener(SetCameraShake);
        }

        private void SetValuesFromOptions()
        {
            masterVolumeSlider.value = OptionsManager.Instance.MasterVolume;
            musicVolumeSlider.value = OptionsManager.Instance.MusicVolume;
            sfxVolumeSlider.value = OptionsManager.Instance.SfxVolume;
            bloomSlider.value = OptionsManager.Instance.BloomEffect;
            grainSlider.value = OptionsManager.Instance.GrainEffect;
            cameraShakeSlider.value = OptionsManager.Instance.CameraShakeEffect;
        }
        
        private void SetMasterVolume(float vol) { OptionsManager.Instance.MasterVolume = vol; }
        private void SetMusicVolume(float vol) { OptionsManager.Instance.MusicVolume = vol; }
        private void SetSfxVolume(float vol) { OptionsManager.Instance.SfxVolume = vol; }
        private void SetBloomEffect(float val) { OptionsManager.Instance.BloomEffect = val; }
        private void SetGrainEffect(float val) { OptionsManager.Instance.GrainEffect = val; }
        private void SetCameraShake(float val) { OptionsManager.Instance.CameraShakeEffect = val; }
    }
}
