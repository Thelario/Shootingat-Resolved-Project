using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace PabloLario.Managers
{
    public class PostProcessingManager : Singleton<PostProcessingManager>
    {
        private Volume _volume;
        private Bloom _bloom;
        private FilmGrain _grain;
        private ColorAdjustments _colorAdjustments;

        [Header("Color Adjustment Values")] 
        [SerializeField] private List<int> _values;
        private int _previousValue;
        
        private void Start()
        {
            _volume = GetComponent<Volume>();
            
            _volume.profile.TryGet(out _bloom);
            _volume.profile.TryGet(out _grain);
            _volume.profile.TryGet(out _colorAdjustments);
        }

        public void SetBloom(float val)
        {
            _bloom.intensity.value = val;
        }

        public void SetGrain(float val)
        {
            _grain.intensity.value = val;
        }

        public void SetColorAdjusments(float val)
        {
            _colorAdjustments.hueShift.value = val;
        }

        public void SetRandomColorAdjustment()
        {
            int newValue;
            do
            { 
                newValue = _values[Random.Range(0, _values.Count)];
            } while (newValue == _previousValue);

            _previousValue = newValue;
            _colorAdjustments.hueShift.value = newValue;
        }
    }
}
