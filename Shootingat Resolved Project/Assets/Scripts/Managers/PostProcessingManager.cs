using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace PabloLario.Managers
{
    public class PostProcessingManager : Singleton<PostProcessingManager>
    {
        private Volume _volume;
        private Bloom _bloom;
        private FilmGrain _grain;
        
        private void Start()
        {
            _volume = GetComponent<Volume>();
            
            _volume.profile.TryGet(out _bloom);
            _volume.profile.TryGet(out _grain);
        }

        public void SetBloom(float val)
        {
            _bloom.intensity.value = val;
        }

        public void SetGrain(float val)
        {
            _grain.intensity.value = val;
        }
    }
}
