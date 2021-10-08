using UnityEngine;

public class BulletsManager : Singleton<BulletsManager>
{
    public GameObject GetBullets(BulletType bt)
    {
        foreach (Bullets b in Assets.Instance.bulletsArray)
        {
            if (b.type == bt)
                return b.bulletPrefab;
        }

        Debug.LogError("Bullet Prefab Not Found");
        return null;
    }
}
