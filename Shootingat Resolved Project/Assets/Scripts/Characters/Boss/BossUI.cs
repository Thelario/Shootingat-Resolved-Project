using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Characters.Boss
{
    public class BossUI : MonoBehaviour
    {
        [SerializeField] private Slider healthSlider;

        public void SetSlider(int value, int maxValue)
        {
            healthSlider.maxValue = maxValue;
            healthSlider.value = value;
        }

        public void UpdateSliderValue(int newValue)
        {
            print(newValue);
            healthSlider.value = newValue;
        }

        public void DisableSlider()
        {
            healthSlider.gameObject.SetActive(false);
        }
    }
}