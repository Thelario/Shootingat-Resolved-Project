using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace PabloLario.Managers
{
    public class PostProcessingManager : Singleton<PostProcessingManager>
    {
        private Volume volume;
        private Bloom bloom;
        private FilmGrain grain;
        
        private void Start()
        {
            volume = GetComponent<Volume>();
            volume.profile.TryGet(out bloom);
            volume.profile.TryGet(out grain);
        }

        public void SetBloom(bool enabled)
        {
            bloom.active = enabled;
        }

        public void SetGrain(bool enabled)
        {
            grain.active = enabled;
        }
    }
}
