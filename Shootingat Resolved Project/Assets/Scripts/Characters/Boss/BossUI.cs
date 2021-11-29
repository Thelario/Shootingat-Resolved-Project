using System;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Characters.Boss
{
    public class BossUI : MonoBehaviour
    {
        private Slider _healthSlider;

        private void Awake()
        {
            _healthSlider = PabloLario.Managers.Assets.Instance.bossHealthSlider;
            _healthSlider.gameObject.SetActive(true);
        }

        public void SetSlider(int value, int maxValue)
        {
            _healthSlider.maxValue = maxValue;
            _healthSlider.value = value;
        }

        public void UpdateSliderValue(int newValue)
        {
            //print(newValue);
            _healthSlider.value = newValue;
        }

        public void DisableSlider()
        {
            _healthSlider.gameObject.SetActive(false);
        }
    }
}