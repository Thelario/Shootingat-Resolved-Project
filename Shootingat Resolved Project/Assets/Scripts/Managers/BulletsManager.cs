using UnityEngine;

namespace PabloLario.Managers
{
    public class BulletsManager : Singleton<BulletsManager>
    {
        public GameObject GetBullets(BulletType bt)
        {
            if (Assets.Instance.bulletsDictionary.TryGetValue(bt, out GameObject bullet))
                return bullet;
            else
                Debug.LogError("Bullet Prefab Not Found");
                return null;
        }
    }
}