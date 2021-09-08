using UnityEngine;

public class BulletsManager : Singleton<BulletsManager>
{
    /// <summary>
    /// GetBullets searchs for the correct bulletPrefab and returns it
    /// </summary>
    /// <param name="pt"> The BulletType that we want to get </param>
    /// <returns> Bullet prefab that we want to get </returns>
    public GameObject GetBullets(BulletType bt)
    {
        foreach (Bullets b in Assets.Instance.bulletsArray)
        {
            if (b.type == bt)
                return b.bulletPrefab;
        }

        Debug.LogError("Particle Prefab Not Found");
        return null;
    }
}
