using UnityEngine;
using PabloLario.Managers;

namespace PabloLario
{
    public class SplashContainer : Singleton<SplashContainer>
    {
        protected override void Awake()
        {
            base.Awake();

            GameManager.OnDungeonGenerated += DeleteSplashes;
        }

        private void OnDestroy()
        {
            GameManager.OnDungeonGenerated -= DeleteSplashes;
        }

        private void DeleteSplashes()
        {
            if (transform.childCount > 0)
            {
                foreach (Transform t in transform)
                    Destroy(t.gameObject);
            }
        }
    }
}
